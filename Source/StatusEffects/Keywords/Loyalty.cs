using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class LoyaltyDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Loyalty);
        }
    }
    [EntityLogic(typeof(LoyaltyDef))]
    public sealed class Loyalty : StatusEffect
    {
    }
}
