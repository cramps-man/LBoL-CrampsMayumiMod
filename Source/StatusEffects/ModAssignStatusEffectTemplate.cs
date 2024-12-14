using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using UnityEngine;

namespace LBoLMod.StatusEffects
{
    public abstract class ModAssignStatusEffectTemplate: ModStatusEffectTemplate
    {
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("assignicon.png", BepinexPlugin.embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            return new StatusEffectConfig(
                Id: "",
                Index: 0,
                Order: 15,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Keep,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: true,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
        }
    }
}
