using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class TaskLevelDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(TaskLevel);
        }
    }
    [EntityLogic(typeof(TaskLevelDef))]
    public sealed class TaskLevel : StatusEffect
    {
    }
}
