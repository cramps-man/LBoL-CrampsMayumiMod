using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class FrontlineLoyaltyGainDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineLoyaltyGain);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Green };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 3 };
            cardConfig.Value1 = 6;
            cardConfig.UpgradedValue1 = 10;
            cardConfig.Keywords = Keyword.Echo;
            cardConfig.UpgradedKeywords = Keyword.Echo;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineLoyaltyGainDef))]
    public sealed class FrontlineLoyaltyGain : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (var frontline in Battle.HandZoneAndPlayArea.Concat(Battle.DrawZone).Concat(Battle.DiscardZone).Where(c => c is ModFrontlineCard).Cast<ModFrontlineCard>())
            {
                frontline.RemainingValue += Value1;
            };
            yield break;
        }
    }
}
