using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class EnterPowerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(EnterPower);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 10;
            cardConfig.UpgradedDamage = 12;
            cardConfig.Cost = new ManaGroup() { Any = 1, Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1 };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(EnterPowerDef))]
    public sealed class EnterPower : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector);
            if (!base.Battle.BattleShouldEnd)
            {
                foreach (var item in StanceUtils.ApplyStance<PowerStance>(base.Battle.Player))
                {
                    yield return item;
                }
            }
        }
    }
}
