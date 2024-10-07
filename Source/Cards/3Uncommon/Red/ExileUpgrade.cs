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
    public sealed class ExileUpgradeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ExileUpgrade);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1, Any = 2 };
            cardConfig.Block = 16;
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Value2 = 1;
            cardConfig.UpgradedValue2 = 2;
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ExileUpgradeDef))]
    public sealed class ExileUpgrade : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c is ModFrontlineCard).ToList();
            return new SelectHandInteraction(0, Value1, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            if (!(precondition is SelectHandInteraction exileInteraction))
                yield break;

            yield return new ExileManyCardAction(exileInteraction.SelectedCards);

            int exileCount = exileInteraction.SelectedCards.Count;
            for (int i = 0; i < exileCount + Value2; i++)
            {
                yield return new UpgradeCardsAction(base.Battle.HandZone.Where(c => c.CanUpgradeAndPositive));
            }
        }
    }
}
