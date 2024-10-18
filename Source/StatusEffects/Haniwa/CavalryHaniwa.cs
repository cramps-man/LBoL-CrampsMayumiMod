using LBoL.ConfigData;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Resource;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using UnityEngine;

namespace LBoLMod.StatusEffects
{
    public sealed class CavalryHaniwaDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CavalryHaniwa);
        }
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("cavalryhaniwa.png", BepinexPlugin.embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.Order = 13;
            statusConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            return statusConfig;
        }
    }

    [EntityLogic(typeof(CavalryHaniwaDef))]
    public sealed class CavalryHaniwa: ModHaniwaStatusEffect
    {
        protected override void OnAdding(Unit unit)
        {
            base.OnAdding(unit);
        }
    }
}
