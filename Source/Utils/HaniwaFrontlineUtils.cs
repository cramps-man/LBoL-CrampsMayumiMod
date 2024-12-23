using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Exhibits.Shining;
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
            typeof(OptionHaniwaMonk),
            typeof(OptionHaniwaCharger),
            typeof(OptionHaniwaSentinel)
        };

        public static readonly List<Type> UncommonSummonTypes = new List<Type>
        {
            typeof(HaniwaUpgrader),
            typeof(HaniwaExploiter),
            typeof(HaniwaSpy),
            typeof(HaniwaMonk),
            typeof(HaniwaCharger),
            typeof(HaniwaSentinel)
        };

        public static List<Type> AllOptionTypes => CommonOptionTypes.Concat(UncommonOptionTypes).ToList();
        public static List<Type> AllSummonTypes => CommonSummonTypes.Concat(UncommonSummonTypes).ToList();

        public static List<Type> GetAllSummonTypes(BattleController battle)
        {
            bool hasGreen = battle.GameRun.BaseMana.HasColor(ManaColor.Green);
            bool hasPurple = battle.GameRun.BaseMana.HasColor(ManaColor.Black);
            bool hasBlankCard = battle.Player.HasExhibit<KongbaiKapai>();
            List<Type> toReturn = new List<Type>();
            if (hasGreen || hasBlankCard)
                toReturn.Add(typeof(HaniwaHorseArcher));
            if (hasPurple || hasBlankCard)
                toReturn.Add(typeof(HaniwaAssassin));
            if (battle.Player.HasStatusEffect<ChainOfCommandSe>())
            {
                toReturn.Add(typeof(FrontlineCommander));
                toReturn.Add(typeof(AssignCommander));
            }
            toReturn.AddRange(AllSummonTypes);
            return toReturn;
        }

        public static List<Card> GetAllOptionCards(BattleController battle, int numberToSpawn = 1, bool checkSacrificeRequirement = false)
        {
            bool hasGreen = battle.GameRun.BaseMana.HasColor(ManaColor.Green);
            bool hasPurple = battle.GameRun.BaseMana.HasColor(ManaColor.Black);
            bool hasBlankCard = battle.Player.HasExhibit<KongbaiKapai>();
            List<Type> toReturn = new List<Type>();
            if (hasGreen || hasBlankCard)
                toReturn.Add(typeof(OptionHaniwaHorseArcher));
            if (hasPurple || hasBlankCard)
                toReturn.Add(typeof(OptionHaniwaAssassin));
            if (battle.Player.HasStatusEffect<ChainOfCommandSe>())
            {
                toReturn.Add(typeof(OptionFrontlineCommander));
                toReturn.Add(typeof(OptionAssignCommander));
            }
            toReturn.AddRange(AllOptionTypes);
            return GetOptionCards(toReturn, battle, numberToSpawn, checkSacrificeRequirement);
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

        public static IEnumerable<BattleAction> CardsSummon(IReadOnlyList<Card> modFrontlineCards, int startingUpgrades = 0, int startingLoyalty = 0)
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
                List<ModFrontlineCard> cards = optionCard.GetCardsToSpawn().Cast<ModFrontlineCard>().ToList();
                if (startingUpgrades > 0)
                    cards.ForEach(c => c.UpgradeCounter = startingUpgrades);
                if (startingLoyalty > 0)
                    cards.ForEach(c => c.StartingExtraLoyalty += startingLoyalty);
                cardsToSpawn.AddRange(cards);
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

        public static IEnumerable<BattleAction> ExecuteOnPlayActions(List<Card> frontlineCards, BattleController battle, UnitSelector selector = null, bool consumeRemainingValue = false)
        {
            foreach (ModFrontlineCard card in frontlineCards)
            {
                card.NotifyActivating();
                var precondition = card.Precondition();
                if (precondition != null)
                {
                    precondition.Description = card.Name;
                    yield return new InteractionAction(precondition, true);
                }
                foreach (var action in card.GetActions(selector != null ? selector : GetTargetForOnPlayAction(battle), ManaGroup.Empty, precondition, new List<DamageAction>(), false))
                {
                    if (battle.BattleShouldEnd)
                        yield break;
                    yield return action;
                }
                if (consumeRemainingValue)
                    yield return card.ConsumeLoyalty();
            }
        }
    }
}
