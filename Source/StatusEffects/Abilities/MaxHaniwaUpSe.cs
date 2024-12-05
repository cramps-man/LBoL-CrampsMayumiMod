using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class MaxHaniwaUpSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MaxHaniwaUpSe);
        }
    }

    [EntityLogic(typeof(MaxHaniwaUpSeDef))]
    public sealed class MaxHaniwaUpSe: StatusEffect
    {
    }
}
