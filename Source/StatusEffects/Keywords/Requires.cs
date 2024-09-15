using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class RequireDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Require);
        }
    }
    [EntityLogic(typeof(RequireDef))]
    public sealed class Require : StatusEffect
    {
    }
}
