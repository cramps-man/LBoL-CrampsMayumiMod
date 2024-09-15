using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Source.StatusEffects.Stances
{
    public sealed class DexterityDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Dexterity);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            return statusConfig;
        }
    }

    [EntityLogic(typeof(DexterityDef))]
    public sealed class Dexterity: StatusEffect
    {
    }
}
