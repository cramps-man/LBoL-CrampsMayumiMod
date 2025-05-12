using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class FencerWorshippingDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FencerWorshipping);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Blue };
            cardConfig.Cost = new ManaGroup() { Blue = 1, Any = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Blue = 1, Any = 1 };
            cardConfig.Value1 = 20;
            cardConfig.Value2 = 5;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FencerWorshippingDef))]
    public sealed class FencerWorshipping : ModMayumiCard
    {
        public int FencerGain => IsUpgraded ? 5 : 3;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<FencerWorshippingSe>(Value1);
            yield return new GainHaniwaAction(fencerToGain: FencerGain);
        }
    }
}