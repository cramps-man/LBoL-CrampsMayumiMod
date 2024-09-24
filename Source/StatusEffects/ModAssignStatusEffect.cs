using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLMod.Exhibits;
using LBoLMod.Source.Cards;
using LBoLMod.UltimateSkills;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public abstract class ModAssignStatusEffect: StatusEffect
    {
        protected ModAssignCard AssignSourceCard { get; set; }
        protected int CardHaniwaRequired => AssignSourceCard.HaniwaRequired;
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
                return TickDown(PlayerHasExhibitA);
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
                return TickDown(false);
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
                return TickDown(PlayerHasExhibitA);
            }
            return null;
        }

        private IEnumerable<BattleAction> TickDown(bool hasExhibitA)
        {
            this.NotifyActivating();
            foreach (var item in OnAssignmentDone())
            {
                yield return item;
                if (base.Battle.BattleShouldEnd)
                    yield break;
            };
            if (hasExhibitA)
            {
                yield return new DrawCardAction();
            }
            yield return new ApplyStatusEffectAction(AssignSourceCard.HaniwaType, base.Battle.Player, AssignSourceCard.HaniwaRequired);
            yield return new RemoveStatusEffectAction(this);
        }
        protected abstract IEnumerable<BattleAction> OnAssignmentDone();
    }
}
