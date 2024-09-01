using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LBoLMod.StatusEffects
{
    public sealed class PowerStanceDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PowerStance);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.StatusEffectsBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("icon.png", BepinexPlugin.directorySource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            return new StatusEffectConfig(
                Id: "",
                Index: 0,
                Order: 10,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: false,
                StackActionTriggerLevel: null,
                HasLevel: false,
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
                SFX: "Default"
            );

        }
    }

    public sealed class PowerStance: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            this.React(new ApplyStatusEffectAction<Firepower>(unit, 1));
        }

        protected override void OnRemoved(Unit unit)
        {
            this.React(new ApplyStatusEffectAction<FirepowerNegative>(unit, 1));
        }
    }
}
