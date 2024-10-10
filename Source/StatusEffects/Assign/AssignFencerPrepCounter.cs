using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

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
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            var player = base.Battle.Player;
            yield return new CastBlockShieldAction(player, player, AssignSourceCard.Block);
        }

        protected override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            var player = base.Battle.Player;
            if (AssignSourceCard.IsUpgraded)
                yield return BuffAction<Reflect>(level: player.Block + player.Shield);
            else
                yield return BuffAction<Reflect>(level: player.Block);
        }
    }
}
