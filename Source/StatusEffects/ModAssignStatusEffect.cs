using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLMod.BattleActions;
using LBoLMod.Cards;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.UltimateSkills;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public abstract class ModAssignStatusEffect: StatusEffect
    {
        private bool JustApplied { get; set; } = true;
        protected ModAssignCard AssignSourceCard { get; set; }
        public int CardFencerAssigned { get; set; } = 0;
        public int CardArcherAssigned { get; set; } = 0;
        public int CardCavalryAssigned { get; set; } = 0;
        protected int StartingCardCounter => AssignSourceCard.StartingCardCounter;
        protected DamageInfo CardDamage => AssignSourceCard.Damage;
        protected int CardBlock => AssignSourceCard.RawBlock;
        protected int CardShield => AssignSourceCard.RawShield;
        protected ManaGroup CardMana => AssignSourceCard.Mana;
        protected int CardValue1 => AssignSourceCard.Value1;
        protected int CardValue2 => AssignSourceCard.Value2;
        protected ScryInfo CardScry => AssignSourceCard.Scry;
        public bool IsPaused { get; set; } = false;
        public bool IsPermanent { get; set; } = false;
        protected override void OnAdded(Unit unit)
        {
            if (SourceCard is ModAssignCard c)
                AssignSourceCard = c;
            CardFencerAssigned += AssignSourceCard.FencerAssigned;
            CardArcherAssigned += AssignSourceCard.ArcherAssigned;
            CardCavalryAssigned += AssignSourceCard.CavalryAssigned;
            this.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStarted));
            base.HandleOwnerEvent(base.Battle.Player.TurnEnded, this.onPlayerTurnEnded);
            base.ReactOwnerEvent(base.Battle.UsUsed, this.OnUltimateSkillUsed);
        }

        public override bool Stack(StatusEffect other)
        {
            base.Stack(other);
            Count += 3;
            CardFencerAssigned += AssignSourceCard.FencerAssigned;
            CardArcherAssigned += AssignSourceCard.ArcherAssigned;
            CardCavalryAssigned += AssignSourceCard.CavalryAssigned;
            if (!SourceCard.IsUpgraded && other.SourceCard.IsUpgraded)
            {
                if (other.SourceCard is ModAssignCard c)
                    AssignSourceCard = c;
            }
            if (IsPermanent)
            {
                Level = 1;
                this.Tickdown(3);
            }
            return true;
        }

        public void MakePermanent()
        {
            IsPermanent = true;
            Level = 1;
        }
        public void IncreaseExtraTrigger(int amount)
        {
            if (IsPermanent)
                Tickdown(3);
            else
                Level += amount;
        }
        public void Tickdown(int amount)
        {
            if (IsPaused)
                return;
            if (base.Battle.Player.HasStatusEffect<AssignReverseTickdownSe>())
            {
                Count += amount;
                return;
            }
            if (Count - amount >= 0)
                Count -= amount;
            else
                Count = 0;
        }

        public IEnumerable<BattleAction> ImmidiatelyTrigger()
        {
            Count = 0;
            return AssignTriggering(false);
        }

        public IEnumerable<BattleAction> DuplicateTrigger(int triggerCount = 1)
        {
            yield return new AssignTriggerAction(OnAssignmentDone(false), BeforeAssignmentDone(false, triggerCount), AfterAssignmentDone(false, triggerCount), triggerCount, false);
        }

        private IEnumerable<BattleAction> OnUltimateSkillUsed(UsUsingEventArgs args)
        {
            if (args.Us is UltimateSkillA)
            {
                return AssignTriggering(false);
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
                return AssignTriggering(true);
            }
            return null;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (!JustApplied)
                Tickdown(1);
            JustApplied = false;
            if (Count == 0)
            {
                return AssignTriggering(false);
            }
            return null;
        }

        private IEnumerable<BattleAction> AssignTriggering(bool onTurnStart, bool shouldRemove = true)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            this.NotifyActivating();
            if (IsPermanent)
                Level = 1;
            yield return new AssignTriggerAction(OnAssignmentDone(onTurnStart), BeforeAssignmentDone(onTurnStart, Level), AfterAssignmentDone(onTurnStart, Level), Level, onTurnStart);
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (IsPermanent)
            {
                Count = StartingCardCounter;
                if (HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaActionType.Require, CardFencerAssigned, CardArcherAssigned, CardCavalryAssigned))
                {
                    yield return new LoseHaniwaAction(HaniwaActionType.Require, CardFencerAssigned, CardArcherAssigned, CardCavalryAssigned);
                }
                else
                {
                    int hpLoss = CardFencerAssigned + CardArcherAssigned + CardCavalryAssigned;
                    yield return new DamageAction(base.Battle.Player, base.Battle.Player, DamageInfo.HpLose(hpLoss));
                }
            }
            else if (shouldRemove)
            {
                yield return new GainHaniwaAction(CardFencerAssigned, CardArcherAssigned, CardCavalryAssigned, true);
                yield return new RemoveStatusEffectAction(this);
            }
        }
        protected abstract IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart);
        protected virtual IEnumerable<BattleAction> BeforeAssignmentDone(bool onTurnStart, int triggerCount)
        {
            return new List<BattleAction>();
        }
        protected virtual IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart, int triggerCount)
        {
            return new List<BattleAction>();
        }
    }
}
