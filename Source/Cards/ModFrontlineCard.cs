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
    public abstract class ModFrontlineCard: Card
    {
        public override string Description => base.Description + UiUtils.WrapByColor(" " + nameof(Frontline), GlobalConfig.DefaultKeywordColor);
        public int RemainingValue { get; set; } = 0;
        protected virtual bool IncludeUpgradesInRemainingValue => false;
        private void SetRemainValue(int value)
        {
            RemainingValue = value;
            base.NotifyChanged();
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<CardsEventArgs>(base.Battle.CardsAddedToHand, new GameEventHandler<CardsEventArgs>(this.OnCardsAddedToHand));
        }

        private void OnCardsAddedToHand(CardsEventArgs args)
        {
            if (args.Cards.Contains(this))
            {
                SetRemainValue(Value1);
            }
        }
        public override IEnumerable<BattleAction> OnDraw()
        {
            SetRemainValue(Value1);
            return null;
        }

        public override IEnumerable<BattleAction> OnTurnStartedInHand()
        {
            SetRemainValue(Value1);
            return null;
        }
        public override int AdditionalValue1 => base.UpgradeCounter.GetValueOrDefault();

        private const int UPGRADE_AMOUNT = 1;
        public override void Upgrade()
        {
            if (IncludeUpgradesInRemainingValue)
            {
                RemainingValue += UPGRADE_AMOUNT;
            }
            else
            {
                RemainingValue += Config.UpgradedValue1.GetValueOrDefault() - Config.Value1.GetValueOrDefault();
            }
            int? upgradeCounter = base.UpgradeCounter + UPGRADE_AMOUNT;
            base.UpgradeCounter = upgradeCounter;
            ProcessKeywordUpgrade();
            CostChangeInUpgrading();
            NotifyChanged();
        }
        public override bool CanUpgrade => base.UpgradeCounter < 99;
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
            base.UpgradeCounter = 0;
        }
    }
}
