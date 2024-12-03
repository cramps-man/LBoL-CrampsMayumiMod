using Cysharp.Threading.Tasks;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.PlayerUnits;
using UnityEngine;

namespace LBoLMod.UnitModel
{
    public sealed class UnitModel : UnitModelTemplate
    {
        public override IdContainer GetId()
        {
            return new PlayerDef().UniqueId;
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.unitModelBatchLoc.AddEntity(this);
        }

        public override ModelOption LoadModelOptions()
        {
            return new ModelOption(ResourceLoader.LoadSpriteAsync("MayumiModel.png", BepinexPlugin.directorySource, ppu: 565));
        }

        public override UniTask<Sprite> LoadSpellSprite()
        {
            return ResourceLoader.LoadSpriteAsync("MayumiStand.png", BepinexPlugin.directorySource);
        }

        public override UnitModelConfig MakeConfig()
        {
            var config = DefaultConfig();
            config.Flip = true;
            config.HasSpellPortrait = true;
            return config;
        }
    }
}
