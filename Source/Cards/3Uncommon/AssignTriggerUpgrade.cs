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
    public sealed class AssignTriggerUpgradeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignTriggerUpgrade);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Value1 = 1;
            cardConfig.Cost = new ManaGroup() { White = 1, Red = 1, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignTriggerUpgradeDef))]
    public sealed class AssignTriggerUpgrade : ModMayumiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return UpgradeAllHandsAction();
            yield return BuffAction<AssignTriggerUpgradeSe>(level: Value1);
        }
    }
}
