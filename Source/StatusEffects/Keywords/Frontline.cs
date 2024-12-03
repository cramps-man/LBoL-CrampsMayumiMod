using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class FrontlineDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Frontline);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.RelativeEffects = new List<string>() { nameof(Loyalty) };
            return statusConfig;
        }
    }
    [EntityLogic(typeof(FrontlineDef))]
    public sealed class Frontline : StatusEffect
    {
    }
}
