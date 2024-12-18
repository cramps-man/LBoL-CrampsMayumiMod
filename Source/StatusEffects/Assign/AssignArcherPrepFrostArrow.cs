using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignArcherPrepFrostArrowDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignArcherPrepFrostArrow);
        }
    }

    [EntityLogic(typeof(AssignArcherPrepFrostArrowDef))]
    public sealed class AssignArcherPrepFrostArrow : ModAssignStatusEffect
    {
        public int TotalTimes => Math.Max(Level / CardValue1, 1);
        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
            base.ReactOwnerEvent(base.Battle.Player.DamageDealt, this.OnDamageDealt);
        }

        private IEnumerable<BattleAction> OnDamageDealt(DamageEventArgs args)
        {
            if (args.ActionSource != this)
                yield break;
            if (!args.Target.HasStatusEffect<Cold>())
                yield break;

            var player = base.Battle.Player;
            yield return new CastBlockShieldAction(player, player, AssignSourceCard.Block);
        }

        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            for (var i = 0; i < TotalTimes; i++)
            {
                var target = base.Battle.RandomAliveEnemy;
                yield return new DamageAction(Owner, target, AssignSourceCard.Damage);
                yield return DebuffAction<Cold>(target);
            }
        }
    }
}
