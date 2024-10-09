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
    public sealed class UltraFrontlineDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(UltraFrontline);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2 };
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(UltraFrontlineDef))]
    public sealed class UltraFrontline : Card
    {
        public override Interaction Precondition()
        {
            List<Card> cards = HaniwaFrontlineUtils.CommonSummonTypes.ConvertAll(t => Library.CreateCard(t)).Concat(HaniwaFrontlineUtils.UncommonSummonTypes.ConvertAll(t => Library.CreateCard(t))).ToList();
            return new SelectCardInteraction(1, 1, cards);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction interaction))
                yield break;

            Card newSummon = interaction.SelectedCards.FirstOrDefault();
            if (newSummon == null)
                yield break;

            List<Card> exileCards = base.Battle.HandZone.Where(c => c is ModFrontlineCard).ToList();
            if (IsUpgraded)
            {
                foreach (Card c in exileCards)
                {
                    newSummon.UpgradeCounter += c.UpgradeCounter + 1;
                }
            }
            else
                newSummon.UpgradeCounter = exileCards.Count;
            if (newSummon.UpgradeCounter > ModFrontlineCard.MAX_UPGRADE)
                newSummon.UpgradeCounter = ModFrontlineCard.MAX_UPGRADE;
            yield return new ExileManyCardAction(exileCards);
            yield return new AddCardsToHandAction(newSummon);
        }
    }
}
