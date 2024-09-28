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
        protected int CardFencerRequired => AssignSourceCard.FencerAssigned;
        protected int CardArcherRequired => AssignSourceCard.ArcherAssigned;
        protected int CardCavalryRequired => AssignSourceCard.CavalryAssigned;
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
            this.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStarted));
            base.HandleOwnerEvent(base.Battle.Player.TurnEnded, this.onPlayerTurnEnded);
            base.ReactOwnerEvent(base.Battle.UsUsed, this.OnUltimateSkillUsed);
        }

        public void Tickdown(int amount)
        {
            if (Count - amount >= 0)
                Count -= amount;
            else
                Count = 0;
        }

        public void TickdownFull()
        {
            Count = 0;
        }

        private IEnumerable<BattleAction> OnUltimateSkillUsed(UsUsingEventArgs args)
        {
            if (args.Us is UltimateSkillA)
            {
                return AssignTriggering(false, PlayerHasExhibitA);
            }
            return null;
        }

        private void onPlayerTurnEnded(UnitEventArgs args)
        {
            Tickdown(3);
        }

        private IEnumerable<BattleAction> onPlayerTurnStarted(UnitEventArgs args)
        {
            if (Count == 0)
            {
                return AssignTriggering(true, false);
            }
            return null;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (args.Card != AssignSourceCard)
                Tickdown(1);
            if (Count == 0)
            {
                return AssignTriggering(false, PlayerHasExhibitA);
            }
            return null;
        }

        private IEnumerable<BattleAction> AssignTriggering(bool onTurnStart, bool hasExhibitA)
        {
            this.NotifyActivating();
            yield return new AssignTriggerAction(OnAssignmentDone(onTurnStart));
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (hasExhibitA)
                yield return new DrawCardAction();
            yield return new GainHaniwaAction(CardFencerRequired, CardArcherRequired, CardCavalryRequired, true);
            yield return new RemoveStatusEffectAction(this);
        }
        protected abstract IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart);
    }
}
