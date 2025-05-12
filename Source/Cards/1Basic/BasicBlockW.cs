using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class BasicBlockGDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BasicBlockW);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.FindInBattle = false;
            cardConfig.Type = CardType.Defense;
            cardConfig.TargetType = TargetType.Self;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Block = 10;
            cardConfig.UpgradedBlock = 13;
            cardConfig.Cost = new ManaGroup() { Any = 1, White = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            cardConfig.Keywords = Keyword.Basic;
            cardConfig.UpgradedKeywords = Keyword.Basic;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BasicBlockGDef))]
    public sealed class BasicBlockW : ModMayumiCard
    {
    }
}
