using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Intentions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignFencerPrepCounterDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignFencerPrepCounter);
        }
    }

    [EntityLogic(typeof(AssignFencerPrepCounterDef))]
    public sealed class AssignFencerPrepCounter : ModAssignStatusEffect
    {
        private List<Unit> enemiesThatAttackedPlayer = new List<Unit>();
        public IEnumerable<Unit> EnemiesThatAttackedPlayerAlive => MarkedEnemies.Any() ? enemiesThatAttackedPlayer.Where(e => e.IsAlive && MarkedEnemies.Contains(e)) : enemiesThatAttackedPlayer.Where(e => e.IsAlive);
        public int EnemiesThatAttackedPlayerCount => EnemiesThatAttackedPlayerAlive.Count();
        public DamageInfo TotalDamage => CardDamage.IncreaseBy(Level / Math.Max(EnemiesThatAttackedPlayerCount, 1));
        public int TotalBlock => CardBlock * base.Battle.AllAliveEnemies.Sum(e => e.Intentions.Where(i => i is AttackIntention).Cast<AttackIntention>().Sum(ai => ai.Times == null ? 1 : ai.Times.GetValueOrDefault()));

        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
            base.ReactOwnerEvent(Owner.TurnEnded, this.OnTurnEnded);
            base.HandleOwnerEvent(Owner.DamageReceived, this.OnDamageReceived);
        }
        
        private IEnumerable<BattleAction> OnTurnEnded(UnitEventArgs args)
        {
            yield return new CastBlockShieldAction(Owner, new BlockInfo(TotalBlock));
        }

        private void OnDamageReceived(DamageEventArgs args)
        {
            if (args.Source is EnemyUnit enemy && !enemiesThatAttackedPlayer.Contains(enemy))
                enemiesThatAttackedPlayer.Add(enemy);
            Level += CardValue1;
        }

        public override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            List<Unit> toAttack = EnemiesThatAttackedPlayerCount > 0 ? EnemiesThatAttackedPlayerAlive.ToList() : new List<Unit>() { MarkedEnemies.Any() ? MarkedEnemies.SampleOrDefault(base.Battle.GameRun.BattleRng) : base.Battle.RandomAliveEnemy };
            yield return new DamageAction(Owner, toAttack, TotalDamage);
        }
    }
}
