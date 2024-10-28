using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLMod.BattleActions;
using LBoLMod.Cards;
using LBoLMod.UltimateSkills;
using LBoLMod.Utils;
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
            this.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStarted));
            base.HandleOwnerEvent(base.Battle.Player.TurnEnded, this.onPlayerTurnEnded);
            base.ReactOwnerEvent(base.Battle.UsUsed, this.OnUltimateSkillUsed);
        }

        public override bool Stack(StatusEffect other)
        {
            base.Stack(other);
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
            if (args.Card != AssignSourceCard)
                Tickdown(1);
            if (Count == 0)
            {
                return AssignTriggering(false);
            }
            return null;
        }

        private IEnumerable<BattleAction> AssignTriggering(bool onTurnStart)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            this.NotifyActivating();
            if (IsPermanent)
                Level = 1;
            yield return new AssignTriggerAction(OnAssignmentDone(onTurnStart), BeforeAssignmentDone(onTurnStart), AfterAssignmentDone(onTurnStart), Level, onTurnStart);
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
                    yield return new DamageAction(base.Battle.Player, base.Battle.Player, DamageInfo.HpLose(1));
                }
            }
            else
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
