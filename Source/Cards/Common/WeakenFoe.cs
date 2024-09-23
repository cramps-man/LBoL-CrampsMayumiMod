using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class WeakenFoeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(WeakenFoe);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Damage = 10;
            cardConfig.UpgradedDamage = 12;
            cardConfig.Value1 = 4;
            cardConfig.UpgradedValue1 = 8;
            cardConfig.Value2 = 1;
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Weak) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Weak) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(WeakenFoeDef))]
    public sealed class WeakenFoe : Card
    {
        public int ArcherRequired => 1;
        public DamageInfo HaniwaDamage
        {
            get
            {
                return this.Damage.IncreaseBy(Value1);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (HaniwaUtils.IsLevelFulfilled<ArcherHaniwa>(base.Battle.Player, ArcherRequired, HaniwaActionType.Require))
            {
                yield return base.AttackAction(selector, HaniwaDamage);
                if (base.Battle.BattleShouldEnd)
                    yield break;
                yield return new ApplyStatusEffectAction<Weak>(selector.SelectedEnemy, 0, Value2);
            }
            else
            {
                yield return base.AttackAction(selector);
            }
        }
    }
}
