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
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(UltraFrontlineDef))]
    public sealed class UltraFrontline : Card
    {
        public override Interaction Precondition()
        {
            List<Card> cards = HaniwaFrontlineUtils.AllSummonTypes.ConvertAll(t => Library.CreateCard(t));
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
                exileCards = exileCards.Concat(base.Battle.DrawZone.Concat(base.Battle.DiscardZone).Where(c => c is ModFrontlineCard)).ToList();
            foreach (Card c in exileCards)
            {
                newSummon.UpgradeCounter += c.UpgradeCounter + 1;
            }
            if (newSummon.UpgradeCounter > ModFrontlineCard.MAX_UPGRADE)
                newSummon.UpgradeCounter = ModFrontlineCard.MAX_UPGRADE;
            yield return new ExileManyCardAction(exileCards);
            yield return new AddCardsToHandAction(newSummon);
        }
    }
}
