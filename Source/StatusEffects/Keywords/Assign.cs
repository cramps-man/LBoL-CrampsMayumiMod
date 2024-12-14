using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class AssignDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Assign);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.RelativeEffects = new List<string>() { nameof(TaskLevel) };
            return statusConfig;
        }
    }
    [EntityLogic(typeof(AssignDef))]
    public sealed class Assign : StatusEffect
    {
    }
}
