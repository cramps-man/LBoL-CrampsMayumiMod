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
            cardConfig.Value1 = 10;
            cardConfig.UpgradedValue1 = 15;
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
            int totalHaniwa = haniwaCount.Sum(h => h.Value);
            if (totalHaniwa > Value1)
            {
                for (int i = 0; i < totalHaniwa - Value1; i++)
                {
                    var rand = haniwaCount.Where(h => h.Value > 0).Sample(base.BattleRng);
                    haniwaCount[rand.Key] -= 1;
                }
            }
            List<Card> cardsToSpawn = new List<Card>();
            for (int i = 0; i < haniwaCount["fencer"]; i++)
                cardsToSpawn.Add(Library.CreateCard(HaniwaFrontlineUtils.FencerTypes.Sample(base.BattleRng)));
            for (int i = 0; i < haniwaCount["archer"]; i++)
                cardsToSpawn.Add(Library.CreateCard(HaniwaFrontlineUtils.ArcherTypes.Sample(base.BattleRng)));
            for (int i = 0; i < haniwaCount["cavalry"]; i++)
                cardsToSpawn.Add(Library.CreateCard(HaniwaFrontlineUtils.CavalryTypes.Sample(base.BattleRng)));

            yield return new AddCardsToDrawZoneAction(cardsToSpawn, DrawZoneTarget.Random);
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, haniwaCount["fencer"], haniwaCount["archer"], haniwaCount["cavalry"]);
        }
    }
}
