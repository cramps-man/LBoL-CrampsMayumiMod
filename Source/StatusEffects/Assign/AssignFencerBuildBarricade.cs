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
        public int TotalBarrier => Level / CardValue1;
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield return new CastBlockShieldAction(base.Battle.Player, 0, TotalBarrier);
        }
    }
}
