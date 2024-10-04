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
        public int CardFencerAssigned => AssignSourceCard.FencerAssigned;
        public int CardArcherAssigned => AssignSourceCard.ArcherAssigned;
        public int CardCavalryAssigned => AssignSourceCard.CavalryAssigned;
        protected int StartingCardCounter => AssignSourceCard.StartingCardCounter;
        protected DamageInfo CardDamage => AssignSourceCard.Damage;
        protected int CardShield => AssignSourceCard.RawShield;
        protected ManaGroup CardMana => AssignSourceCard.Mana;
        protected int CardValue1 => AssignSourceCard.Value1;
        protected int CardValue2 => AssignSourceCard.Value2;
        public bool IsPaused { get; set; } = false;
        public int PauseGenFencer { get; set; } = 0;
        public int PauseGenArcher { get; set; } = 0;
        public int PauseGenCavalry { get; set; } = 0;
        private bool PlayerHasExhibitA => base.Battle.Player.HasExhibit<ExhibitA>();
        protected override void OnAdded(Unit unit)
        {
            Level = 1;
            if (SourceCard is ModAssignCard c)
                AssignSourceCard = c;
            this.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStarted));
            base.HandleOwnerEvent(base.Battle.Player.TurnEnded, this.onPlayerTurnEnded);
            base.ReactOwnerEvent(base.Battle.UsUsed, this.OnUltimateSkillUsed);
        }

        public void Tickdown(int amount)
        {
            if (IsPaused)
                return;
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
            IsPaused = false;
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
            yield return new AssignTriggerAction(OnAssignmentDone(onTurnStart), Level, onTurnStart);
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (hasExhibitA)
                yield return new DrawCardAction();
            yield return new GainHaniwaAction(CardFencerAssigned + PauseGenFencer, CardArcherAssigned + PauseGenArcher, CardCavalryAssigned + PauseGenCavalry, true);
            yield return new RemoveStatusEffectAction(this);
        }
        protected abstract IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart);
    }
}
