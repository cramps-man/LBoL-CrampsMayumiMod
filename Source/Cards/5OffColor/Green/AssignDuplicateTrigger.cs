using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignDuplicateTriggerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignDuplicateTrigger);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Green };
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 1;
            cardConfig.UpgradedValue2 = 2;
            cardConfig.Cost = new ManaGroup() { Red = 1, Green = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 9 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignDuplicateTriggerDef))]
    public sealed class AssignDuplicateTrigger : Card
    {
        public override Interaction Precondition()
        {
            return new SelectCardInteraction(1, Value1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player));
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction assignInteraction))
                yield break;

            foreach (ModAssignOptionCard optionCard in assignInteraction.SelectedCards)
            {
                foreach (var item in optionCard.StatusEffect.DuplicateTrigger(Value2))
                {
                    yield return item;
                }
            }
        }
    }
}
