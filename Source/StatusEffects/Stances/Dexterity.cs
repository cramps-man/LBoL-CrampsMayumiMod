using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLMod.StatusEffects;

namespace LBoLMod.Source.StatusEffects.Stances
{
    public sealed class DexterityDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Dexterity);
        }
    }

    public sealed class Dexterity: StatusEffect
    {
    }
}
