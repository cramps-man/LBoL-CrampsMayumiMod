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
    public sealed class AssignCostTaskLevelDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCostTaskLevel);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Green };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 9, Any = 3 };
            cardConfig.UpgradedCost = new ManaGroup() { Hybrid = 1, HybridColor = 9, Any = 1 };
            cardConfig.Value1 = 3;
            cardConfig.Mana = new ManaGroup() { Any = 1 };
            cardConfig.RelativeKeyword = Keyword.TempMorph;
            cardConfig.UpgradedRelativeKeyword = Keyword.TempMorph;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignCostTaskLevelDef))]
    public sealed class AssignCostTaskLevel : ModMayumiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<AssignCostTaskLevelSe>(Value1);
        }
    }
}
