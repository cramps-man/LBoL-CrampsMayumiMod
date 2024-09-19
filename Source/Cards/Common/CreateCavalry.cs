using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
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
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 0 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Value2 = 1;
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
            foreach (var item in HaniwaUtils.GainHaniwa<CavalryHaniwa>(base.Battle.Player, Value1))
            {
                yield return item;
            };
            yield return new DrawCardAction();
        }
    }
}
