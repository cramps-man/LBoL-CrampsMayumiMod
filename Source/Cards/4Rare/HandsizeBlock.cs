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
    public sealed class HandsizeBlockDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HandsizeBlock);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 0 };
            cardConfig.Block = 0;
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 4;
            cardConfig.Value2 = 2;
            cardConfig.Mana = new ManaGroup() { Any = 1 };
            cardConfig.UpgradedKeywords = Keyword.Retain;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HandsizeBlockDef))]
    public sealed class HandsizeBlock : ModMayumiCard
    {
        public override int AdditionalBlock
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                return base.Battle.HandZoneAndPlayArea.Count * Value1;
            }
        }

        public int CurrentNon0costHandsize
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                return base.Battle.HandZoneAndPlayArea.Where(c => c.BaseCost != ManaGroup.Empty && c != this).Count();
            }
        }

        public override ManaGroup AdditionalCost
        {
            get
            {
                if (base.Battle == null)
                    return ManaGroup.Empty;
                return ManaGroup.Anys(CurrentNon0costHandsize / Value2);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
        }
    }
}
