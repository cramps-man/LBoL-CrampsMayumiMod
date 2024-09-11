using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;

namespace LBoLMod.Source.StatusEffects.Keywords
{
    public sealed class StanceDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Stance);
        }
    }
    [EntityLogic(typeof(StanceDef))]
    public sealed class Stance : StatusEffect
    {
    }
}
