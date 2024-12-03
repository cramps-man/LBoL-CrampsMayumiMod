using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class HaniwaMonkDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaMonk);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Damage = 12;
            cardConfig.Value1 = 5;
            cardConfig.Value2 = 2;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaMonkDef))]
    public sealed class HaniwaMonk : ModFrontlineCard
    {
        public override int AdditionalDamage => base.UpgradeCounter.GetValueOrDefault() + ChargedDamage;
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault() / 5;
        public int ChargedDamage => DeltaInt;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.HandleBattleEvent(base.Battle.Player.StatusEffectAdded, this.OnPlayerStatusEffectAdded);
        }

        private void OnPlayerStatusEffectAdded(StatusEffectApplyEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                return;
            if (base.Zone != CardZone.Hand)
                return;
            if (args.Effect.Type != StatusEffectType.Positive)
                return;
            if (RemainingValue <= 0)
                return;

            base.NotifyActivating();
            DeltaInt += Value2;
            RemainingValue -= 1;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            DeltaInt = 0;
            yield return ConsumeLoyalty();
        }
    }
}
