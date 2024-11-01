using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class ZeroCostReductionDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ZeroCostReduction);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2, Red = 2, Any = 1 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.RelativeKeyword = Keyword.TempMorph;
            cardConfig.UpgradedRelativeKeyword = Keyword.TempMorph;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ZeroCostReductionDef))]
    public sealed class ZeroCostReduction : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<ZeroCostReductionSe>(level: Value1, count: Value1);
        }
    }
}
