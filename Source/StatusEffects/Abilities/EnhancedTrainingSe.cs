using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class EnhancedTrainingSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(EnhancedTrainingSe);
        }
    }

    [EntityLogic(typeof(EnhancedTrainingSeDef))]
    public sealed class EnhancedTrainingSe: StatusEffect
    {
    }
}
