using LBoL.Base;
using LBoL.Base.Extensions;
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
using System;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public abstract class ModAssignStatusEffect: StatusEffect
    {
        public bool JustApplied { get; set; } = true;
        public ModAssignCard AssignSourceCard { get; set; }
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
            this.ReactOwnerEvent(base.Battle.CardUsed, this.OnCardUsed);
            base.ReactOwnerEvent(base.Battle.Player.TurnStarted, this.onPlayerTurnStarted);
            base.HandleOwnerEvent(base.Battle.Player.TurnEnded, this.onPlayerTurnEnded);
            base.ReactOwnerEvent(base.Battle.UsUsed, this.OnUltimateSkillUsed);
        }

        public override bool Stack(StatusEffect other)
        {
            if (other.SourceCard is ModAssignCard mac && !mac.ShouldStack)
                return false;
            base.Stack(other);
            Count += 3;
            if (!SourceCard.IsUpgraded && other.SourceCard.IsUpgraded)
            {
                if (other.SourceCard is ModAssignCard c)
                    AssignSourceCard = c;
            }
            CardFencerAssigned += AssignSourceCard.FencerAssigned > 0 ? 1 : 0;
            CardArcherAssigned += AssignSourceCard.ArcherAssigned > 0 ? 1 : 0;
            CardCavalryAssigned += AssignSourceCard.CavalryAssigned > 0 ? 1 : 0;
            return true;
        }

        public void MakePermanent()
        {
            IsPermanent = true;
        }
        public void IncreaseTaskLevel(int amount)
        {
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

        private IEnumerable<BattleAction> OnUltimateSkillUsed(UsUsingEventArgs args)
        {
            if (args.Us is AssignUltimateSkill)
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
            AssignSourceCard.ManualStack = true;
            if (!JustApplied)
                Tickdown(1);
            if (Count == 0)
            {
                JustApplied = false;
                return AssignTriggering(false);
            }
            if (!JustApplied)
            {
                int assignCostTriggerIncrease = base.Battle.Player.TryGetStatusEffect<AssignCostTaskLevelSe>(out AssignCostTaskLevelSe se) ? se.Level : 0;
                Level += Math.Ceiling((CardFencerAssigned + CardArcherAssigned + CardCavalryAssigned) / 2d).RoundToInt() + assignCostTriggerIncrease;
            }
            JustApplied = false;
            return null;
        }

        private IEnumerable<BattleAction> AssignTriggering(bool onTurnStart, bool shouldRemove = true)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            this.NotifyActivating();
            yield return new AssignTriggerAction(OnAssignmentDone(onTurnStart), BeforeAssignmentDone(onTurnStart), AfterAssignmentDone(onTurnStart), Level, onTurnStart);
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (IsPermanent)
            {
                if (HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaActionType.Require, CardFencerAssigned, CardArcherAssigned, CardCavalryAssigned))
                {
                    yield return new LoseHaniwaAction(HaniwaActionType.Require, CardFencerAssigned, CardArcherAssigned, CardCavalryAssigned);
                }
                else
                {
                    int hpLoss = CardFencerAssigned + CardArcherAssigned + CardCavalryAssigned;
                    yield return new DamageAction(base.Battle.Player, base.Battle.Player, DamageInfo.HpLose(hpLoss));
                }
                Count = 5;
                Level = AssignSourceCard.StartingTaskLevel;
                CardFencerAssigned = AssignSourceCard.FencerAssigned;
                CardArcherAssigned = AssignSourceCard.ArcherAssigned;
                CardCavalryAssigned = AssignSourceCard.CavalryAssigned;
            }
            else if (shouldRemove)
            {
                yield return new GainHaniwaAction(CardFencerAssigned, CardArcherAssigned, CardCavalryAssigned, true);
                yield return new RemoveStatusEffectAction(this);
            }
        }
        protected abstract IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart);
        protected virtual IEnumerable<BattleAction> BeforeAssignmentDone(bool onTurnStart)
        {
            return new List<BattleAction>();
        }
        protected virtual IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            return new List<BattleAction>();
        }
    }
}
