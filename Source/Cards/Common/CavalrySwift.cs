using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class CavalrySwiftDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CavalrySwift);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalrySwiftDef))]
    public sealed class CavalrySwift : Card
    {
        public int AssignRequirement => 2;
        public int FrontlineRequirement => 3;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new DrawManyCardAction(Value1);
            var assignStatuses = base.Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect).ToList().Count;
            if (assignStatuses >= AssignRequirement)
                yield return new DrawCardAction();
            var frontlineCards = base.Battle.HandZone.Where((Card c) => c is ModFrontlineCard).ToList().Count;
            if (frontlineCards >= FrontlineRequirement)
                yield return new DrawCardAction();
        }
    }
}
