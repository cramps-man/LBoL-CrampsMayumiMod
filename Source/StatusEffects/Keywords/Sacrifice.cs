using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class SacrificeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Sacrifice);
        }
    }
    [EntityLogic(typeof(SacrificeDef))]
    public sealed class Sacrifice : StatusEffect
    {
    }
}
