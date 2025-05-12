using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class MudExtractionDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MudExtraction);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Blue };
            cardConfig.Cost = new ManaGroup() { Blue = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 5;
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.None;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.RelativeCards = new List<string>() { nameof(UManaCard) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(UManaCard) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(MudExtractionDef))]
    public sealed class MudExtraction : ModMayumiCard
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c != this && c is ModFrontlineCard).ToList();
            return new SelectHandInteraction(1, Value1, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction frontlineInteraction))
                yield break;

            foreach(ModFrontlineCard card in frontlineInteraction.SelectedCards)
            {
                yield return card.ConsumeLoyalty(Value2);
            }
            yield return new AddCardsToHandAction(Library.CreateCards<UManaCard>(frontlineInteraction.SelectedCards.Count));
        }
    }
}
