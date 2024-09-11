using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class BasicAttackRDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BasicAttackR);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.FindInBattle = false;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 10;
            cardConfig.UpgradedDamage = 14;
            cardConfig.Cost = new ManaGroup() { Any = 1, Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            cardConfig.Keywords = Keyword.Basic;
            cardConfig.UpgradedKeywords = Keyword.Basic;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BasicAttackRDef))]
    public sealed class BasicAttackR : Card
    {
    }
}
