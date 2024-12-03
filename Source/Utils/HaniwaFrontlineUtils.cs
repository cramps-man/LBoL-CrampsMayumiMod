using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLMod.BattleActions;
using LBoLMod.Cards;
using LBoLMod.StatusEffects.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Utils
{
    public static class HaniwaFrontlineUtils
    {
        public static readonly List<Type> CommonOptionTypes = new List<Type>
        {
            typeof(OptionHaniwaAttacker),
            typeof(OptionHaniwaBodyguard),
            typeof(OptionHaniwaSharpshooter),
            typeof(OptionHaniwaSupport)
        };

        public static readonly List<Type> CommonSummonTypes = new List<Type>
        {
            typeof(HaniwaAttacker),
            typeof(HaniwaBodyguard),
            typeof(HaniwaSharpshooter),
            typeof(HaniwaSupport)
        };

        public static readonly List<Type> UncommonOptionTypes = new List<Type>
        {
            typeof(OptionHaniwaUpgrader),
            typeof(OptionHaniwaExploiter),
            typeof(OptionHaniwaSpy),
            typeof(OptionHaniwaMonk)
        };

        public static readonly List<Type> UncommonSummonTypes = new List<Type>
        {
            typeof(HaniwaUpgrader),
            typeof(HaniwaExploiter),
            typeof(HaniwaSpy),
            typeof(HaniwaMonk)
        };

        public static List<Type> AllOptionTypes => CommonOptionTypes.Concat(UncommonOptionTypes).ToList();
        public static List<Type> AllSummonTypes => CommonSummonTypes.Concat(UncommonSummonTypes).ToList();

        public static List<Card> GetCommonCards(BattleController battle, int numberToSpawn = 1, bool checkSacrificeRequirement = false)
        {
            return GetOptionCards(CommonOptionTypes, battle, numberToSpawn, checkSacrificeRequirement);
        }

        public static List<Card> GetUncommonCards(BattleController battle, int numberToSpawn = 1, bool checkSacrificeRequirement = false)
        {
            return GetOptionCards(UncommonOptionTypes, battle, numberToSpawn, checkSacrificeRequirement);
        }

        public static List<Card> GetAllCards(BattleController battle, int numberToSpawn = 1, bool checkSacrificeRequirement = false)
        {
            return GetOptionCards(AllOptionTypes, battle, numberToSpawn, checkSacrificeRequirement);
        }

        private static List<Card> GetOptionCards(List<Type> types, BattleController battle, int numberToSpawn = 1, bool checkSacrificeRequirement = false)
        {
            List<Card> cards = new List<Card>();
            foreach (var type in types)
            {
                var c = Library.CreateCard(type) as ModFrontlineOptionCard;
                c.SetBattle(battle);
                c.NumberToSpawn = numberToSpawn;
                if (checkSacrificeRequirement)
                {
                    if (c.FulfilsRequirement)
                    {
                        cards.Add(c);
                    }
                }
                else
                {
                    cards.Add(c);
                }
            }
            return cards;
        }

        public static IEnumerable<BattleAction> CardsSummon(IReadOnlyList<Card> modFrontlineCards, int startingUpgrades = 0)
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
                if (startingUpgrades > 0)
                {
                    var cards = optionCard.GetCardsToSpawn();
                    cards.ForEach(c => c.UpgradeCounter = startingUpgrades);
                    cardsToSpawn.AddRange(cards);
                }
                else
                    cardsToSpawn.AddRange(optionCard.GetCardsToSpawn());
            }
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, totalFencerCost, totalArcherCost, totalCavalryCost);
            yield return new AddCardsToHandAction(cardsToSpawn);
        }

        public static UnitSelector GetTargetForOnPlayAction(BattleController battle)
        {
            var markedEnemy = battle.AllAliveEnemies.Where(e => e.HasStatusEffect<CommandersMarkSe>()).FirstOrDefault();
            if (markedEnemy != null)
            {
                return new UnitSelector(markedEnemy);
            }
            return new UnitSelector(battle.RandomAliveEnemy);
        }

        public static IEnumerable<BattleAction> ExecuteOnPlayActions(List<Card> frontlineCards, BattleController battle, UnitSelector selector = null)
        {
            foreach (ModFrontlineCard card in frontlineCards)
            {
                card.NotifyActivating();
                card.ShouldConsumeRemainingValue = false;
                foreach (var action in card.GetActions(selector != null ? selector : GetTargetForOnPlayAction(battle), ManaGroup.Empty, null, new List<DamageAction>(), false))
                {
                    if (battle.BattleShouldEnd)
                        yield break;
                    yield return action;
                }
            }
        }
    }
}
