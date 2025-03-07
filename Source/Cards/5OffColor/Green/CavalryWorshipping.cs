using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CavalryWorshippingDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CavalryWorshipping);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Green };
            cardConfig.Cost = new ManaGroup() { Green = 1, Any = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Green = 1, Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 5;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Graze) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Graze) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalryWorshippingDef))]
    public sealed class CavalryWorshipping : Card
    {
        public int CavalryGain => IsUpgraded ? 5 : 3;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<CavalryWorshippingSe>(Value1);
            yield return new GainHaniwaAction(cavalryToGain: CavalryGain);
        }
    }
}