using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class CommandDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Command);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.Keywords = LBoL.Base.Keyword.Exile;
            statusConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Loyalty) };
            return statusConfig;
        }
    }
    [EntityLogic(typeof(CommandDef))]
    public sealed class Command : StatusEffect
    {
    }
}
