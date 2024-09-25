using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLMod.BattleActions;
using LBoLMod.Cards;
using LBoLMod.Exhibits;
using LBoLMod.UltimateSkills;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public abstract class ModAssignStatusEffect: StatusEffect
    {
        protected ModAssignCard AssignSourceCard { get; set; }
        protected int CardFencerRequired => AssignSourceCard.FencerRequired;
        protected int CardArcherRequired => AssignSourceCard.ArcherRequired;
        protected int CardCavalryRequired => AssignSourceCard.CavalryRequired;
        protected int StartingCardCounter => AssignSourceCard.StartingCardCounter;
        protected DamageInfo CardDamage => AssignSourceCard.Damage;
        protected int CardShield => AssignSourceCard.RawShield;
        protected ManaGroup CardMana => AssignSourceCard.Mana;
        protected int CardValue1 => AssignSourceCard.Value1;
        private bool PlayerHasExhibitA => base.Battle.Player.HasExhibit<ExhibitA>();
        protected override void OnAdded(Unit unit)
        {
            if (SourceCard is ModAssignCard c)
                AssignSourceCard = c;
            this.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
            this.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStarted));
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnEnded, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnEnded));
            base.ReactOwnerEvent(base.Battle.UsUsed, this.OnUltimateSkillUsed);
        }

        private IEnumerable<BattleAction> OnUltimateSkillUsed(UsUsingEventArgs args)
        {
            if (args.Us is UltimateSkillA)
            {
                return AssignTriggering(PlayerHasExhibitA);
            }
            return null;
        }

        private IEnumerable<BattleAction> onPlayerTurnEnded(UnitEventArgs args)
        {
            if (Count <= 3)
                Count = 0;
            else
                Count -= 3;
            yield break;
        }

        private IEnumerable<BattleAction> onPlayerTurnStarted(UnitEventArgs args)
        {
            if (Count == 0)
            {
                return AssignTriggering(false);
            }
            return null;
        }

        private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
        {
            Count--;
            yield break;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (Count == 0)
            {
                return AssignTriggering(PlayerHasExhibitA);
            }
            return null;
        }

        private IEnumerable<BattleAction> AssignTriggering(bool hasExhibitA)
        {
            this.NotifyActivating();
            yield return new AssignTriggerAction(OnAssignmentDone());
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (hasExhibitA)
            {
                yield return new DrawCardAction();
            }
            yield return new GainHaniwaAction(CardFencerRequired, CardArcherRequired, CardCavalryRequired, true);
            yield return new RemoveStatusEffectAction(this);
        }
        protected abstract IEnumerable<BattleAction> OnAssignmentDone();
    }
}
