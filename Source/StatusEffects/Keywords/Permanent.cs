using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class PermanentDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Permanent);
        }
    }
    [EntityLogic(typeof(PermanentDef))]
    public sealed class Permanent : StatusEffect
    {
    }
}
