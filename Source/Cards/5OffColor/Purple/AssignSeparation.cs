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
    public sealed class AssignSeparationDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignSeparation);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, Any = 2, HybridColor = 7 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentBonusSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentBonusSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignSeparationDef))]
    public sealed class AssignSeparation : ModMayumiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<AssignSeparationSe>();
        }
    }
}
