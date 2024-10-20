using LBoL.Core;
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
        public string BlockBarrierText => StringDecorator.Decorate(AssignSourceCard.IsUpgraded ? "|e:" + CardShield + "| |Barrier|" : "|e:" + CardBlock + "| |Block|");
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            var player = base.Battle.Player;
            if (AssignSourceCard.IsUpgraded)
                yield return new CastBlockShieldAction(player, player, AssignSourceCard.Shield);
            else
                yield return new CastBlockShieldAction(player, player, AssignSourceCard.Block);
        }

        protected override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            var player = base.Battle.Player;
            yield return BuffAction<Reflect>(level: player.Block + player.Shield);
        }
    }
}
