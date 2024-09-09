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
    public sealed class PowerHitDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PowerHit);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 10;
            cardConfig.UpgradedDamage = 12;
            cardConfig.Value1 = 4;
            cardConfig.UpgradedValue1 = 8;
            cardConfig.Cost = new ManaGroup() { Any = 1, Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1, Red = 1 };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(PowerHitDef))]
    public sealed class PowerHit : Card
    {
        public DamageInfo StanceDamage
        {
            get
            {
                return this.Damage.IncreaseBy(Value1);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (StanceUtils.isStanceFulfilled<PowerStance>(base.Battle.Player))
            {
                yield return base.AttackAction(selector, StanceDamage);
                yield return StanceUtils.RemoveDexterityIfNeeded<PowerStance>(base.Battle.Player);
            }
            else
            {
                yield return base.AttackAction(selector);
            }
        }
    }
}
