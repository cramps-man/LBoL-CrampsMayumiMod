using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignBuildWatchtowerDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignBuildWatchtower);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.RelativeEffects = new List<string>() { nameof(Watchtower) };
            return statusConfig;
        }
    }

    [EntityLogic(typeof(AssignBuildWatchtowerDef))]
    public sealed class AssignBuildWatchtower : ModAssignStatusEffect
    {
        protected override IEnumerable<BattleAction> OnAssignmentDone()
        {
            yield return new ApplyStatusEffectAction<Watchtower>(Owner, CardValue1);
        }
    }
}
