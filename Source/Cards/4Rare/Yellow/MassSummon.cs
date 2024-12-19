using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class MassSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MassSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 3 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1 };
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(MassSummonDef))]
    public sealed class MassSummon : Card
    {
        public override bool CanUse => HaniwaUtils.HasAnyHaniwa(base.Battle.Player);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int fencerCount = HaniwaUtils.GetHaniwaLevel<FencerHaniwa>(base.Battle.Player);
            int archerCount = HaniwaUtils.GetHaniwaLevel<ArcherHaniwa>(base.Battle.Player);
            int cavalryCount = HaniwaUtils.GetHaniwaLevel<CavalryHaniwa>(base.Battle.Player);
            Dictionary<string, int> haniwaCount = new Dictionary<string, int>()
            {
                { "fencer", fencerCount },
                { "archer", archerCount },
                { "cavalry", cavalryCount },
            };
            Dictionary<string, int> haniwaSpent = new Dictionary<string, int>()
            {
                { "fencer", 0 },
                { "archer", 0 },
                { "cavalry", 0 },
            };
            List<Card> cardsToSpawn = new List<Card>();
            bool continueRandomizing = true;
            while (continueRandomizing)
            {
                var allOptions = HaniwaFrontlineUtils.GetAllOptionCards(base.Battle).Cast<ModFrontlineOptionCard>().ToList();
                while (true)
                {
                    var randomSelection = allOptions.SampleOrDefault(base.BattleRng);
                    if (randomSelection == null)
                    {
                        continueRandomizing = false;
                        break;
                    }
                    allOptions.Remove(randomSelection);
                    if (randomSelection.SelectRequireFencer > haniwaCount["fencer"])
                        continue;
                    if (randomSelection.SelectRequireArcher > haniwaCount["archer"])
                        continue;
                    if (randomSelection.SelectRequireCavalry > haniwaCount["cavalry"])
                        continue;
                    cardsToSpawn.AddRange(randomSelection.GetCardsToSpawn());
                    haniwaCount["fencer"] -= randomSelection.SelectRequireFencer;
                    haniwaCount["archer"] -= randomSelection.SelectRequireArcher;
                    haniwaCount["cavalry"] -= randomSelection.SelectRequireCavalry;
                    haniwaSpent["fencer"] += randomSelection.SelectRequireFencer;
                    haniwaSpent["archer"] += randomSelection.SelectRequireArcher;
                    haniwaSpent["cavalry"] += randomSelection.SelectRequireCavalry;
                    break;
                }
            }

            yield return new AddCardsToDrawZoneAction(cardsToSpawn, DrawZoneTarget.Random);
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, haniwaSpent["fencer"], haniwaSpent["archer"], haniwaSpent["cavalry"]);
        }
    }
}
