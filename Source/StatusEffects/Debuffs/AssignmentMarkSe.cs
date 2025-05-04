using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Debuffs
{
    public sealed class AssignmentMarkSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignmentMarkSe);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.Type = StatusEffectType.Negative;
            config.HasLevel = false;
            config.HasDuration = true;
            config.DurationDecreaseTiming = DurationDecreaseTiming.TurnEnd;
            config.VFX = "DebuffBlue";
            return config;
        }
    }

    [EntityLogic(typeof(AssignmentMarkSeDef))]
    public sealed class AssignmentMarkSe: StatusEffect
    {
    }
}
