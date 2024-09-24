using LBoL.ConfigData;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public sealed class ArcherHaniwaDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ArcherHaniwa);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.Order = 12;
            statusConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            return statusConfig;
        }
    }

    [EntityLogic(typeof(ArcherHaniwaDef))]
    public sealed class ArcherHaniwa: ModHaniwaStatusEffect
    {
        protected override void OnAdding(Unit unit)
        {
            base.OnAdding(unit);
        }
    }
}
