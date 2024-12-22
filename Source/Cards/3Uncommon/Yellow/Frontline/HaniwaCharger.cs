using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class HaniwaChargerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaCharger);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Damage = 6;
            cardConfig.Value1 = 4;
            cardConfig.Value2 = 0;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaChargerDef))]
    public sealed class HaniwaCharger : ModFrontlineCard
    {
        public override bool IsCavalryType => true;
        protected override bool ShouldConsumeAll => true;
        protected override int OnPlayConsumedRemainingValue => 10;
        public override int AdditionalDamage => RemainingValue + base.UpgradeCounter.GetValueOrDefault();
        public int VulnScaling => 10;
        public override int AdditionalValue2 => RemainingValue / VulnScaling;
        public int TotalLoyaltyGain => BaseLoyaltyGain * ChargerCount;
        public int ChargerCount => base.Battle != null ? base.Battle.HandZone.Where(c => c is HaniwaCharger).Count() : 0;
        public int BaseLoyaltyGain => 2 + base.UpgradeCounter.GetValueOrDefault() / LoyaltyGainScaling;
        public int LoyaltyGainScaling => 5;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.HandleBattleEvent(base.Battle.Player.TurnEnded, this.OnPlayerTurnEnded);
        }

        private void OnPlayerTurnEnded(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                return;
            if (base.Zone != CardZone.Hand)
                return;
            if (TotalLoyaltyGain <= 0)
                return;
            base.NotifyActivating();
            RemainingValue += TotalLoyaltyGain;
            base.NotifyChanged();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (Value2 > 0)
                yield return DebuffAction<Vulnerable>(selector.GetEnemy(base.Battle), duration: Value2);
        }
    }
}
