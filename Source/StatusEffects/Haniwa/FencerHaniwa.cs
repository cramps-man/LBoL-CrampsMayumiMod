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
    public sealed class FencerHaniwaDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FencerHaniwa);
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("fencerhaniwa.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.Order = 11;
            statusConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            return statusConfig;
        }
    }

    [EntityLogic(typeof(FencerHaniwaDef))]
    public sealed class FencerHaniwa: ModHaniwaStatusEffect
    {
        protected override void OnAdding(Unit unit)
        {
            base.OnAdding(unit);
        }
    }
}
