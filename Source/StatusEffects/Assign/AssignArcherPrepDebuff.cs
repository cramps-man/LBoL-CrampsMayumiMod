using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignArcherPrepDebuffDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignArcherPrepDebuff);
        }
    }

    [EntityLogic(typeof(AssignArcherPrepDebuffDef))]
    public sealed class AssignArcherPrepDebuff : ModAssignStatusEffect
    {
        public int DivideDmg => 2;
        public int TotalDmg => Level / DivideDmg;
        public int TotalVuln => Math.Max(Level / CardValue1, 1);
        public int TotalLockon => Math.Max(Level / CardValue2, 1);
        public override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            var accurateDmg = DamageInfo.Attack(TotalDmg, true);
            yield return new DamageAction(Owner, base.Battle.AllAliveEnemies, accurateDmg);
            foreach (var item in DebuffAction<Vulnerable>(Battle.AllAliveEnemies, duration: TotalVuln))
            {
                yield return item;
            };
            foreach (var item in DebuffAction<LockedOn>(Battle.AllAliveEnemies, level: TotalLockon))
            {
                yield return item;
            };
        }
    }
}
