using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System;
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
            cardConfig.Damage = 5;
            cardConfig.Value1 = 10;
            cardConfig.Value2 = 0;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Vulnerable) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Vulnerable) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaChargerDef))]
    public sealed class HaniwaCharger : ModFrontlineCard
    {
        public override bool IsCavalryType => true;
        protected override bool ShouldConsumeAll => true;
        protected override int OnPlayConsumedRemainingValue => 5;
        public int HalfLoyalty => RemainingValue / 2;
        public override int AdditionalDamage => Math.Max(0, HalfLoyalty) + base.UpgradeCounter.GetValueOrDefault();
        public int VulnScaling => 30;
        public override int AdditionalValue2 => RemainingValue >= VulnScaling ? 1 : 0;
        public int TotalLoyaltyGain => BaseLoyaltyGain + ChargerCount;
        public int ChargerCount => base.Battle != null ? base.Battle.HandZone.Where(c => c is HaniwaCharger).Count() : 0;
        public int BaseLoyaltyGain => 4 + base.UpgradeCounter.GetValueOrDefault() / LoyaltyGainScaling;
        public int LoyaltyGainScaling => 3;
        public int AutoChargeThreshold => 30 + base.UpgradeCounter.GetValueOrDefault();
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.TurnStarting, this.OnPlayerTurnStarting);
            base.HandleBattleEvent(base.Battle.Player.TurnEnded, this.OnPlayerTurnEnding);
        }

        private void OnPlayerTurnEnding(UnitEventArgs args)
        {
            RemainingValue += TotalLoyaltyGain;
            base.NotifyChanged();
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;
            if (TotalLoyaltyGain <= 0)
                yield break;

            if (RemainingValue >= AutoChargeThreshold)
            {
                yield return PerformAction.Wait(0.2f);
                base.NotifyActivating();
                foreach (var action in GetActions(new UnitSelector(base.Battle.LowestHpEnemy), ManaGroup.Empty, null, false, false, new List<DamageAction>()))
                {
                    yield return action;
                }
                yield return ConsumeLoyalty();
                yield return new DiscardAction(this);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (Value2 > 0)
                yield return DebuffAction<Vulnerable>(selector.GetEnemy(base.Battle), duration: Value2);
        }
    }
}
