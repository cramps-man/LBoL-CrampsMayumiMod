using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Intentions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
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
        List<Unit> enemiesThatAttackedPlayer = new List<Unit>();
        public int enemiesThatAttackedPlayerCount => enemiesThatAttackedPlayer.Count;
        public DamageInfo totalDamage => CardDamage.MultiplyBy(Level);
        public int totalBlock => CardBlock * base.Battle.AllAliveEnemies.Sum(e => e.Intentions.Where(i => i is AttackIntention).Cast<AttackIntention>().Sum(ai => ai.Times == null ? 1 : ai.Times.GetValueOrDefault()));

        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
            base.ReactOwnerEvent(Owner.TurnEnded, this.OnTurnEnded);
            base.HandleOwnerEvent(Owner.DamageReceived, this.OnDamageReceived);
        }

        private IEnumerable<BattleAction> OnTurnEnded(UnitEventArgs args)
        {
            yield return new CastBlockShieldAction(Owner, new BlockInfo(totalBlock));
        }

        private void OnDamageReceived(DamageEventArgs args)
        {
            if (args.Source is EnemyUnit enemy && !enemiesThatAttackedPlayer.Contains(enemy))
                enemiesThatAttackedPlayer.Add(enemy);
            Level++;
        }

        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield break;
        }

        protected override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart, int triggerCount)
        {
            yield return new DamageAction(Owner, enemiesThatAttackedPlayer, totalDamage);
        }
    }
}
