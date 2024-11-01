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
            cardConfig.Cost = new ManaGroup() { White = 1, Red = 1, Any = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 3, Hybrid = 1, HybridColor = 2 };
            cardConfig.Block = 0;
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 4;
            cardConfig.Mana = new ManaGroup() { Any = 0 };
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
                return CurrentHandsize == base.Battle.MaxHand ? ManaGroup.Empty - base.BaseCost : Mana;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
        }
    }
}
