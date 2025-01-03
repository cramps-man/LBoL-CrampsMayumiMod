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
    public sealed class AssignTriggerSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignTriggerSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Value1 = 1;
            cardConfig.Cost = new ManaGroup() { White = 2, Red = 2, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1, Red = 1, Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign), nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(Frontline) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaUpgrader), nameof(HaniwaExploiter) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaUpgrader), nameof(HaniwaExploiter) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignTriggerSummonDef))]
    public sealed class AssignTriggerSummon : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<AssignTriggerSummonSe>(Value1);
        }
    }
}
