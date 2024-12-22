using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Helpers;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public abstract class ModFrontlineCard : Card
    {
        public override string Description => Keywords == Keyword.None ? base.Description + "\n" + UiUtils.WrapByColor(nameof(Frontline), GlobalConfig.DefaultKeywordColor) : base.Description + UiUtils.WrapByColor(" " + nameof(Frontline), GlobalConfig.DefaultKeywordColor);

        private int _remainingValue = 0;
        public int RemainingValue
        {
            get
            {
                if (IsDarknessMode)
                    return 10;
                return _remainingValue;
            }
            set
            {
                _remainingValue = value;
                base.NotifyChanged();
            }
        }
        protected virtual bool IncludeUpgradesInRemainingValue => true;
        protected virtual int PassiveConsumedRemainingValue => 1;
        protected virtual int OnPlayConsumedRemainingValue => 1;
        public bool ShouldConsumeRemainingValue { get; set; } = true;
        protected virtual bool ShouldConsumeAll => false;
        public virtual bool IsFencerType => false;
        public virtual bool IsArcherType => false;
        public virtual bool IsCavalryType => false;
        public int StartingExtraLoyalty { get; set; } = 0;
        public bool IsDarknessMode { get; set; } = false;
        protected override void OnEnterBattle(BattleController battle)
        {
            RemainingValue = Value1 + StartingExtraLoyalty;
            base.HandleBattleEvent(base.Battle.CardsAddedToHand, this.OnCardsAddedToHand);
            base.HandleBattleEvent(base.Battle.CardUsing, this.OnCardUsing);
            base.HandleBattleEvent(base.Battle.CardMoved, this.OnCardMoved);
            base.HandleBattleEvent(base.Battle.CardMovedToDrawZone, this.OnCardMovedToDrawZone);
            base.HandleBattleEvent(ModGameEvents.GainedHaniwa, this.OnGainedHaniwa);
        }

        private void OnGainedHaniwa(GainHaniwaEventArgs args)
        {
            if (!base.Battle.HandZone.Contains(this))
                return;
            if (IsFencerType && args.FencerToGain > 0)
                RemainingValue += Math.Ceiling(args.FencerToGain / 2d).RoundToInt();
            if (IsArcherType && args.ArcherToGain > 0)
                RemainingValue += Math.Ceiling(args.ArcherToGain / 2d).RoundToInt();
            if (IsCavalryType && args.CavalryToGain > 0)
                RemainingValue += Math.Ceiling(args.CavalryToGain / 2d).RoundToInt();
        }

        private void OnCardMovedToDrawZone(CardMovingToDrawZoneEventArgs args)
        {
            if (args.Card != this)
                return;
            if (args.SourceZone != CardZone.Exile)
                return;
            RemainingValue = Value1;
        }

        private void OnCardMoved(CardMovingEventArgs args)
        {
            if (args.Card != this)
                return;
            if (args.SourceZone != CardZone.Exile)
                return;
            RemainingValue = Value1;
        }

        private void OnCardUsing(CardUsingEventArgs args)
        {
            ShouldConsumeRemainingValue = true;
        }

        private void OnCardsAddedToHand(CardsEventArgs args)
        {
            if (args.Cards.Contains(this))
            {
                RemainingValue = Value1 + StartingExtraLoyalty;
            }
        }

        public BattleAction ConsumeLoyalty(int loyaltyOverride = -1)
        {
            if (!ShouldConsumeRemainingValue)
                return null;
            if (IsDarknessMode)
            {
                int powerToLose = 10;
                if (powerToLose > base.Battle.Player.Power)
                    return new RemoveCardAction(this);
                return new LosePowerAction(powerToLose);
            }
            if (loyaltyOverride > -1)
                RemainingValue -= loyaltyOverride;
            else if (ShouldConsumeAll && RemainingValue > OnPlayConsumedRemainingValue)
                RemainingValue = 0;
            else
                RemainingValue -= OnPlayConsumedRemainingValue;
            if (RemainingValue < 0)
                return new ExileCardAction(this);
            return null;
        }
        public bool CheckPassiveLoyaltyNotFulfiled(int toCheck = -1)
        {
            if (IsDarknessMode)
                return false;
            if (toCheck > -1)
                return RemainingValue < toCheck;
            return RemainingValue < PassiveConsumedRemainingValue;
        }

        public BattleAction ConsumePassiveLoyalty(int toConsume = -1)
        {
            if (IsDarknessMode)
            {
                int powerToLose = toConsume > -1 ? toConsume : PassiveConsumedRemainingValue;
                if (powerToLose > base.Battle.Player.Power)
                    return new RemoveCardAction(this);
                return new LosePowerAction(powerToLose);
            }
            if (toConsume > -1)
                RemainingValue -= toConsume;
            else
                RemainingValue -= PassiveConsumedRemainingValue;
            return null;
        }
        public BattleAction CheckKeepInHand()
        {
            if (IsDarknessMode && Zone != CardZone.None)
                return new MoveCardAction(this, CardZone.Hand);
            return null;
        }
        public override IEnumerable<BattleAction> AfterUseAction()
        {
            yield return ConsumeLoyalty();
            yield return CheckKeepInHand();
            foreach (var battleAction in base.AfterUseAction())
            {
                yield return battleAction;
            };
        }
        public override int AdditionalValue1 
        {
            get
            {
                if (IncludeUpgradesInRemainingValue)
                    return base.UpgradeCounter.GetValueOrDefault();
                else
                    return 0;
            }
        }

        private const int UPGRADE_AMOUNT = 1;
        public override void Upgrade()
        {
            int? upgradeCounter = base.UpgradeCounter + UPGRADE_AMOUNT;
            base.UpgradeCounter = upgradeCounter;
            RemainingValue += UPGRADE_AMOUNT;
            ProcessKeywordUpgrade();
            CostChangeInUpgrading();
            NotifyChanged();
        }
        public const int MAX_UPGRADE = 999;
        public override bool CanUpgrade => base.UpgradeCounter < MAX_UPGRADE;
        public override bool IsUpgraded
        {
            get
            {
                int? upgradeCounter = base.UpgradeCounter;
                if (upgradeCounter.HasValue)
                {
                    return upgradeCounter.GetValueOrDefault() > 0;
                }

                return false;
            }
        }
        public override void Initialize()
        {
            base.Initialize();
            RemainingValue = Value1;
            base.UpgradeCounter = 0;
        }
    }
}
