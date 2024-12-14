using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignFencerBuildBarricadeDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignFencerBuildBarricade);
        }
    }

    [EntityLogic(typeof(AssignFencerBuildBarricadeDef))]
    public sealed class AssignFencerBuildBarricade : ModAssignStatusEffect
    {
        public int TotalBarrier => AssignSourceCard.Shield.Shield * Level;
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield break;
        }
        protected override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart, int triggerCount)
        {
            yield return new CastBlockShieldAction(base.Battle.Player, 0, TotalBarrier);
        }
    }
}
