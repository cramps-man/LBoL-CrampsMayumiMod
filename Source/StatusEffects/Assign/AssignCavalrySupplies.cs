using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignCavalrySuppliesDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCavalrySupplies);
        }
    }

    [EntityLogic(typeof(AssignCavalrySuppliesDef))]
    public sealed class AssignCavalrySupplies : ModAssignStatusEffect
    {
        protected override IEnumerable<BattleAction> OnAssignmentDone()
        {
            yield return new GainManaAction(new ManaGroup() { Red = 1, White = 1 });
            yield return HaniwaUtils.ForceGainHaniwa<CavalryHaniwa>(base.Battle.Player, HaniwaAssigned);
        }
    }
}
