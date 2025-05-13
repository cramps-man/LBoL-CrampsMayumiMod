using LBoL.Base.Extensions;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
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
        public int TotalTimes => Math.Max(Level / CardValue1, 1);
        public override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            for (var i = 0; i < TotalTimes; i++)
            {
                if (base.Battle.BattleShouldEnd)
                    yield break;
                var markedEnemy = MarkedEnemies.SampleOrDefault(base.Battle.GameRun.BattleRng);
                yield return new DamageAction(Owner, markedEnemy != null ? markedEnemy : base.Battle.RandomAliveEnemy, AssignSourceCard.Damage);
            }
        }
    }
}
