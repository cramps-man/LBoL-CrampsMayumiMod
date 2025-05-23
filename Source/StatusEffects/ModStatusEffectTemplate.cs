﻿using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using UnityEngine;

namespace LBoLMod.StatusEffects
{
    public abstract class ModStatusEffectTemplate: StatusEffectTemplate
    {
        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.StatusEffectsBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            var sprite = ResourceLoader.LoadSprite(GetId() + ".png", BepinexPlugin.embeddedSource);
            if (sprite == null)
            {
                return ResourceLoader.LoadSprite("dummyicon.png", BepinexPlugin.embeddedSource);
            }
            return sprite;
        }

        public override StatusEffectConfig MakeConfig()
        {
            return new StatusEffectConfig(
                Id: "",
                Index: 0,
                Order: 17,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default",
                ImageId: null
            );

        }
    }
}
