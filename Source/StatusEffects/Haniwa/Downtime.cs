using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public sealed class DowntimeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Downtime);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.Type = StatusEffectType.Negative;
            statusConfig.IsStackable = false;
            statusConfig.HasLevel = false;
            statusConfig.HasDuration = true;
            statusConfig.DurationDecreaseTiming = DurationDecreaseTiming.TurnStart;
            statusConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            return statusConfig;
        }
    }

    [EntityLogic(typeof(DowntimeDef))]
    public sealed class Downtime: StatusEffect
    {
    }
}
