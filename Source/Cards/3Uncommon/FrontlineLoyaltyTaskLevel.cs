using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class FrontlineLoyaltyTaskLevelDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineLoyaltyTaskLevel);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 2, Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineLoyaltyTaskLevelDef))]
    public sealed class FrontlineLoyaltyTaskLevel : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<FrontlineLoyaltyTaskLevelSe>();
        }
    }
}
