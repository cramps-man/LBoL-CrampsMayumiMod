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
    public sealed class AssignPlayDrawDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignPlayDraw);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 3, Any = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 2, Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 4;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentBonusSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentBonusSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignPlayDrawDef))]
    public sealed class AssignPlayDraw : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<AssignPlayDrawSe>(Value1);
            yield return BuffAction<AssignmentBonusSe>(Value2);
        }
    }
}
