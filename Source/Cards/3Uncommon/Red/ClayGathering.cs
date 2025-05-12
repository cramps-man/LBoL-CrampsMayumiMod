using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class ClayGatheringDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ClayGathering);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1, Any = 1 };
            cardConfig.Block = 8;
            cardConfig.UpgradedBlock = 10;
            cardConfig.Shield = 0;
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ClayGatheringDef))]
    public sealed class ClayGathering : ModMayumiCard
    {
        public override int AdditionalShield
        {
            get
            {
                if (base.Battle != null)
                    return base.Battle.Player.StatusEffects.Where(s => s.Type == StatusEffectType.Positive).Count() * Value1;
                return 0;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
        }
    }
}
