using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignCavalryRushDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCavalryRush);
        }
    }

    [EntityLogic(typeof(AssignCavalryRushDef))]
    public sealed class AssignCavalryRush : ModAssignStatusEffect
    {
        public DamageInfo TotalDamage => CardDamage.MultiplyBy(Level);

        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
            base.ReactOwnerEvent(base.Battle.EnemyDied, this.OnEnemyDied);
        }

        private IEnumerable<BattleAction> OnEnemyDied(DieEventArgs args)
        {
            if (args.ActionSource != this)
                yield break;
            if (base.Battle.BattleShouldEnd)
                yield break;

            yield return new GainManaAction(CardMana);
            yield return new DamageAction(Owner, base.Battle.LowestHpEnemy, TotalDamage);
        }

        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield return new DamageAction(Owner, base.Battle.LowestHpEnemy, TotalDamage);
        }
    }
}
