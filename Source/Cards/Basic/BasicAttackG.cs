using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class BasicAttackGDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BasicAttackG);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.FindInBattle = false;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Green };
            cardConfig.Damage = 10;
            cardConfig.UpgradedDamage = 14;
            cardConfig.Cost = new ManaGroup() { Any = 1, Green = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            cardConfig.Keywords = Keyword.Basic;
            cardConfig.UpgradedKeywords = Keyword.Basic;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BasicAttackGDef))]
    public sealed class BasicAttackG : Card
    {
    }
}
