using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.Utils;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class SummonHaniwaDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(SummonHaniwa);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1 };
            cardConfig.Value1 = 2;
            cardConfig.Value2 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(SummonHaniwaDef))]
    public sealed class SummonHaniwa : Card
    {
        public override Interaction Precondition()
        {
            List<Card> cards = HaniwaFrontlineUtils.GetAllCards(base.Battle, Value1, true);
            return new MiniSelectCardInteraction(cards);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is MiniSelectCardInteraction interaction))
                yield break;

            ModFrontlineOptionCard optionCard = interaction.SelectedCard as ModFrontlineOptionCard;
            if (optionCard == null)
                yield break;

            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, optionCard.SelectRequireFencer, optionCard.SelectRequireArcher, optionCard.SelectRequireCavalry);
            yield return new AddCardsToHandAction(optionCard.GetCardsToSpawn());
        }
    }
}
