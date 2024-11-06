using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Helpers;
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
                return _remainingValue;
            }
            set
            {
                _remainingValue = value;
                base.NotifyChanged();
            }
        }
        public override ManaGroup AdditionalCost
        {
            get
            {
                if (base.Battle == null)
                    return ManaGroup.Empty;
                return base.Battle.HandZone.Contains(this) && RemainingValue <= 0 ? ManaGroup.Anys(1) : ManaGroup.Empty;
            }
        }
        protected virtual bool IncludeUpgradesInRemainingValue => false;

        protected override void OnEnterBattle(BattleController battle)
        {
            RemainingValue = Value1;
            base.HandleBattleEvent(base.Battle.CardsAddedToHand, this.OnCardsAddedToHand);
        }

        private void OnCardsAddedToHand(CardsEventArgs args)
        {
            if (args.Cards.Contains(this))
            {
                RemainingValue = Value1;
            }
        }
        public override IEnumerable<BattleAction> OnDraw()
        {
            RemainingValue = Value1;
            return null;
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
            /*if (IncludeUpgradesInRemainingValue)
            {
                RemainingValue += UPGRADE_AMOUNT;
            }
            else
            {
                RemainingValue += Config.UpgradedValue1.GetValueOrDefault() - Config.Value1.GetValueOrDefault();
            }*/
            int? upgradeCounter = base.UpgradeCounter + UPGRADE_AMOUNT;
            base.UpgradeCounter = upgradeCounter;
            RemainingValue = Value1;
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
