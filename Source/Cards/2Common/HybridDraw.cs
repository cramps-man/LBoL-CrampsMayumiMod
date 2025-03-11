using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class HybridDrawDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HybridDraw);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign), nameof(AssignmentBonusSe), nameof(LoyaltyProtectionSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HybridDrawDef))]
    public sealed class HybridDraw : Card
    {
        public int AssignRequirement => 1;
        public int FrontlineRequirement => 1;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new DrawManyCardAction(Value1);
            var assignStatuses = base.Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect).Count();
            if (assignStatuses >= AssignRequirement)
            {
                yield return new DrawCardAction();
                if (IsUpgraded)
                    yield return BuffAction<AssignmentBonusSe>(Value1);
            }
            var frontlineCards = base.Battle.HandZone.Where((Card c) => c is ModFrontlineCard).Count();
            if (frontlineCards >= FrontlineRequirement)
            {
                yield return new DrawCardAction();
                if (IsUpgraded)
                    yield return BuffAction<LoyaltyProtectionSe>(Value2);
            }
        }
    }
}
