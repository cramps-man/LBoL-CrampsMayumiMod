using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CreateCavalryDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CreateCavalry);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Value2 = 1;
            cardConfig.Value2 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(CavalryHaniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(CavalryHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateCavalryDef))]
    public sealed class CreateCavalry : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainHaniwaAction(typeof(CavalryHaniwa), Value1);
            yield return new DrawManyCardAction(Value2);
        }
    }
}
