using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class FrontlineDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Frontline);
        }
    }
    [EntityLogic(typeof(FrontlineDef))]
    public sealed class Frontline : StatusEffect
    {
    }
}
