using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class NumBuffBlockDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(NumBuffBlock);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2, White = 1 };
            cardConfig.Block = 10;
            cardConfig.UpgradedBlock = 15;
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 4;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(NumBuffBlockDef))]
    public sealed class NumBuffBlock : Card
    {
        public override int AdditionalBlock
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
