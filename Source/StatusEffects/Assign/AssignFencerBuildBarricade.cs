using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
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
    public sealed class AssignFencerBuildBarricade : ModAssignStatusEffect
    {
        protected override IEnumerable<BattleAction> OnAssignmentDone()
        {
            yield return new CastBlockShieldAction(base.Battle.Player, 0, Level);
            yield return HaniwaUtils.ForceGainHaniwa<FencerHaniwa>(base.Battle.Player, HaniwaAssigned);
        }
    }
}
