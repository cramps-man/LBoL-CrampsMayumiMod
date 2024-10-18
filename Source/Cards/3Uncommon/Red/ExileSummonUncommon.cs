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
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class ExileSummonUncommonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ExileSummonUncommon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ExileSummonUncommonDef))]
    public sealed class ExileSummonUncommon : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c != this && c is ModFrontlineCard).ToList();
            return new SelectHandInteraction(1, Value1, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction exileInteraction))
                yield break;

            int exileCount = exileInteraction.SelectedCards.Count;
            var summonInteraction = new SelectCardInteraction(0, exileCount, HaniwaFrontlineUtils.AllSummonTypes.ConvertAll(t => Library.CreateCard(t)));
            yield return new InteractionAction(summonInteraction);

            yield return new ExileManyCardAction(exileInteraction.SelectedCards);
            yield return new AddCardsToHandAction(summonInteraction.SelectedCards);

        }
    }
}
