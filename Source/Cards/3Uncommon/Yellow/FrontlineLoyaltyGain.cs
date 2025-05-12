using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 1, White = 1 };
            cardConfig.Value1 = 4;
            cardConfig.UpgradedValue1 = 8;
            cardConfig.Value2 = 2;
            cardConfig.UpgradedValue2 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(TempFirepower), nameof(TempSpirit) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(TempFirepower), nameof(TempSpirit) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineLoyaltyGainDef))]
    public sealed class FrontlineLoyaltyGain : ModMayumiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (var frontline in Battle.HandZoneAndPlayArea.Concat(Battle.DrawZone).Concat(Battle.DiscardZone).Where(c => c is ModFrontlineCard).Cast<ModFrontlineCard>())
            {
                frontline.RemainingValue += Value1;
            };
            yield return BuffAction<TempFirepower>(Value2);
            yield return BuffAction<TempSpirit>(Value2);
        }
    }
}
