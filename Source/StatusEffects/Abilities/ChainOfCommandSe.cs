using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class ChainOfCommandSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ChainOfCommandSe);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasLevel = false;
            return config;
        }
    }

    [EntityLogic(typeof(ChainOfCommandSeDef))]
    public sealed class ChainOfCommandSe: StatusEffect
    {
    }
}
