using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class LeadThroughExampleDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LeadThroughExample);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 2, Hybrid = 3, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2, Hybrid = 2, HybridColor = 2 };
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 5;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign), nameof(LoyaltyProtectionSe), nameof(AssignmentBonusSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign), nameof(LoyaltyProtectionSe), nameof(AssignmentBonusSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(LeadThroughExampleDef))]
    public sealed class LeadThroughExample : ModMayumiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<LeadThroughExampleSe>(Value1);
        }
    }
}
