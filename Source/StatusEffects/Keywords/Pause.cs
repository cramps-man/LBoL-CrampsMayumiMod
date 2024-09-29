using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class PauseDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Pause);
        }
    }
    [EntityLogic(typeof(PauseDef))]
    public sealed class Pause : StatusEffect
    {
    }
}
