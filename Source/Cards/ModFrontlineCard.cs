using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Helpers;
using LBoLMod.StatusEffects.Keywords;
using System;
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

        protected override void OnEnterBattle(BattleController battle)
        {
            RemainingValue = Value1;
            base.HandleBattleEvent(base.Battle.CardsAddedToHand, this.OnCardsAddedToHand);
            base.HandleBattleEvent(base.Battle.CardPlaying, this.OnCardPlaying);
        }

        private void OnCardPlaying(CardUsingEventArgs args)
        {
            ShouldConsumeRemainingValue = true;
        }

        protected BattleAction ConsumeLoyalty()
        {
            if (!ShouldConsumeRemainingValue)
                return null;
            RemainingValue -= OnPlayConsumedRemainingValue;
            if (RemainingValue < 0)
                return new ExileCardAction(this);
            return null;
        }

        private void OnCardsAddedToHand(CardsEventArgs args)
        {
            if (args.Cards.Contains(this))
            {
                RemainingValue = Value1;
            }
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
