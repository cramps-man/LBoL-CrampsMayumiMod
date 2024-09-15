using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class AssignDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Assign);
        }
    }
    [EntityLogic(typeof(AssignDef))]
    public sealed class Assign : StatusEffect
    {
    }
}
