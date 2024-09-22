using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class CreateReservesDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CreateReserves);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeCards = new List<string>() { nameof(CreateHaniwa) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(CreateHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateReservesDef))]
    public sealed class CreateReserves : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card hand) => hand != this).ToList();
            if (list.Count <= 0)
            {
                return null;
            }
            return new SelectHandInteraction(0, Value1, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition == null)
                yield break;
            IReadOnlyList<Card> cards = ((SelectHandInteraction)precondition).SelectedCards;
            yield return new ExileManyCardAction(cards);
            yield return new AddCardsToHandAction(Library.CreateCards<CreateHaniwa>(cards.Count));
        }
    }
}
