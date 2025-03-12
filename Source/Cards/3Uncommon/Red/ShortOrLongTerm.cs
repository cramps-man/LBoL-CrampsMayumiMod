using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class ShortOrLongTermDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ShortOrLongTerm);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 2;
            cardConfig.UpgradedValue2 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentBonusSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentBonusSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ShortOrLongTermDef))]
    public sealed class ShortOrLongTerm : Card
    {
        public override Interaction Precondition()
        {
            List<ShortOrLongTerm> list = Library.CreateCards<ShortOrLongTerm>(2, IsUpgraded).ToList();
            ShortOrLongTerm shortChoice = list[0];
            ShortOrLongTerm longChoice = list[1];
            shortChoice.ChoiceCardIndicator = 1;
            longChoice.ChoiceCardIndicator = 2;
            shortChoice.SetBattle(base.Battle);
            longChoice.SetBattle(base.Battle);
            return new MiniSelectCardInteraction(list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is MiniSelectCardInteraction selectInteraction))
                yield break;
            Card choiceCard = selectInteraction.SelectedCard;
            if (choiceCard != null)
            {
                if (choiceCard.ChoiceCardIndicator == 1)
                {
                    var assignInteraction = new SelectCardInteraction(0, Value1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player));
                    yield return new InteractionAction(assignInteraction);
                    foreach (ModAssignOptionCard assignCard in assignInteraction.SelectedCards)
                    {
                        foreach (var item in assignCard.StatusEffect.ImmidiatelyTrigger())
                        {
                            yield return item;
                        }
                    }
                }
                else
                {
                    yield return BuffAction<AssignmentBonusSe>(Value2);
                }
            }
        }
    }
}
