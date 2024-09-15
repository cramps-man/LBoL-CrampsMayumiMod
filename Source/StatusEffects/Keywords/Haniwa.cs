using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Keywords
{
    public sealed class HaniwaDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Haniwa);
        }
    }
    [EntityLogic(typeof(HaniwaDef))]
    public sealed class Haniwa : StatusEffect
    {
    }
}
