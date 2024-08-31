using Cysharp.Threading.Tasks;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LBoLMod.PlayerUnits;

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
            return new ModelOption(ResourceLoader.LoadSpriteAsync("icon.png", BepinexPlugin.directorySource, ppu: 565));
        }

        public override UniTask<Sprite> LoadSpellSprite()
        {
            return ResourceLoader.LoadSpriteAsync("icon.png", BepinexPlugin.directorySource);
        }

        public override UnitModelConfig MakeConfig()
        {
            return DefaultConfig();
        }
    }
}
