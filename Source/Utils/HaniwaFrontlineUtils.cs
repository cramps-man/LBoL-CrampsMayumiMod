using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLMod.BattleActions;
using LBoLMod.Cards;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.Source.Utils
{
    public static class HaniwaFrontlineUtils
    {
        public static readonly List<Type> CommonSummonTypes = new List<Type>
        {
            typeof(OptionHaniwaAttacker),
            typeof(OptionHaniwaBodyguard),
            typeof(OptionHaniwaSharpshooter),
            typeof(OptionHaniwaSupport)
        };

        public static List<Card> GetCommonCards(BattleController battle, int numberToSpawn = 1, bool checkSacrificeRequirement = false)
        {
            List<Card> cards = new List<Card>();
            foreach (var type in HaniwaFrontlineUtils.CommonSummonTypes)
            {
                var c = Library.CreateCard(type) as ModFrontlineOptionCard;
                c.SetBattle(battle);
                c.NumberToSpawn = numberToSpawn;
                if (checkSacrificeRequirement)
                    if (c.FulfilsRequirement)
                        cards.Add(c);
                else cards.Add(c);
            }
            return cards;
        }

        public static IEnumerable<BattleAction> CardsSummon(IReadOnlyList<Card> modFrontlineCards)
        {
            int totalFencerCost = 0;
            int totalArcherCost = 0;
            int totalCavalryCost = 0;
            List<Card> cardsToSpawn = new List<Card>();
            foreach (var card in modFrontlineCards)
            {
                ModFrontlineOptionCard optionCard = card as ModFrontlineOptionCard;
                if (optionCard == null)
                    continue;

                totalFencerCost += optionCard.SelectRequireFencer;
                totalArcherCost += optionCard.SelectRequireArcher;
                totalCavalryCost += optionCard.SelectRequireCavalry;
                cardsToSpawn.AddRange(optionCard.GetCardsToSpawn());
            }
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, totalFencerCost, totalArcherCost, totalCavalryCost);
            yield return new AddCardsToHandAction(cardsToSpawn);
        }
    }
}
