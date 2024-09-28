using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignArcherPrepVolleyDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignArcherPrepVolley);
        }
    }

    [EntityLogic(typeof(AssignArcherPrepVolleyDef))]
    public sealed class AssignArcherPrepVolley : ModAssignStatusEffect
    {
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            for (int i = 0; i < AssignSourceCard.Value1; i++)
            {
                yield return new DamageAction(Owner, base.Battle.RandomAliveEnemy, AssignSourceCard.Damage);
            }
        }
    }
}
