using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class PreserveDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Preserve);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            return statusConfig;
        }
    }
    [EntityLogic(typeof(PreserveDef))]
    public sealed class Preserve : StatusEffect
    {
    }
}
