using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

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
            cardConfig.Value1 = 4;
            cardConfig.Value2 = 2;
            cardConfig.UpgradedValue2 = 3;
            cardConfig.Mana = new ManaGroup() { Any = 1 };
            cardConfig.UpgradedKeywords = Keyword.Retain;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HandsizeBlockDef))]
    public sealed class HandsizeBlock : Card
    {
        public override int AdditionalBlock
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                return CurrentHandsize * Value1;
            }
        }

        public int CurrentHandsize
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                return base.Battle.HandZoneAndPlayArea.Count;
            }
        }

        public override ManaGroup AdditionalCost
        {
            get
            {
                if (base.Battle == null)
                    return ManaGroup.Empty;
                return ManaGroup.Anys(CurrentHandsize / Value2);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
        }
    }
}
