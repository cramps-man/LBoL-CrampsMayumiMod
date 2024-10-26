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
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class UpgradedSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(UpgradedSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 7;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(UpgradedSummonDef))]
    public sealed class UpgradedSummon : Card
    {
        public override Interaction Precondition()
        {
            List<Card> cards = HaniwaFrontlineUtils.GetAllCards(base.Battle, checkSacrificeRequirement: true);
            return new SelectCardInteraction(1, 1, cards);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction interaction))
                yield break;

            foreach (ModFrontlineOptionCard optionCard in interaction.SelectedCards)
            {
                List<Card> cards = optionCard.GetCardsToSpawn();
                cards.ForEach(c => c.UpgradeCounter = Value1);

                yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, optionCard.SelectRequireFencer, optionCard.SelectRequireArcher, optionCard.SelectRequireCavalry);
                yield return new AddCardsToHandAction(cards);
            }
        }
    }
}
