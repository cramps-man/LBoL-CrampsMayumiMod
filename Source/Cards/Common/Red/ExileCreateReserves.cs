using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class ExileCreateReservesDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ExileCreateReserves);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.RelativeCards = new List<string>() { nameof(CreateHaniwa) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(CreateHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ExileCreateReservesDef))]
    public sealed class ExileCreateReserves : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card hand) => hand is ModFrontlineCard).ToList();
            return new SelectHandInteraction(0, Value1, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            IReadOnlyList<Card> cards = ((SelectHandInteraction)precondition).SelectedCards;
            yield return new ExileManyCardAction(cards);
            yield return new AddCardsToHandAction(Library.CreateCards<CreateHaniwa>(cards.Count));
            yield return new UpgradeCardsAction(base.Battle.HandZone.Where((Card hand) => hand is ModFrontlineCard));
        }
    }
}
