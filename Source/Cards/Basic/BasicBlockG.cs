using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class BasicBlockGDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BasicBlockG);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.FindInBattle = false;
            cardConfig.Type = CardType.Defense;
            cardConfig.TargetType = TargetType.Self;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Green };
            cardConfig.Block = 10;
            cardConfig.UpgradedBlock = 13;
            cardConfig.Cost = new ManaGroup() { Any = 1, Green = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BasicBlockGDef))]
    public sealed class BasicBlockG : Card
    {
    }
}
