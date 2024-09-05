using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.StatusEffects;
using System.Collections.Generic;
using UnityEngine;

namespace LBoLMod.UltimateSkills
{
    public sealed class UltimateSkillADef: UltimateSkillTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(UltimateSkillA);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.ultimateSkillBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("icon.png", BepinexPlugin.directorySource);
        }

        public override UltimateSkillConfig MakeConfig()
        {
            var config = new UltimateSkillConfig(
                Id: "",
                Order: 10,
                PowerCost: 100,
                PowerPerLevel: 100,
                MaxPowerLevel: 2,
                RepeatableType: UsRepeatableType.OncePerTurn,
                Damage: 0,
                Value1: 0,
                Value2: 0,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
                );

            return config;

        }
    }

    [EntityLogic(typeof(UltimateSkillADef))]
    public sealed class UltimateSkillA : UltimateSkill
    {
        public UltimateSkillA() {
            base.TargetType = TargetType.Self;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            var unit = base.Battle.Player;
            yield return StanceUtils.RemoveStance<PowerStance>(unit);
            yield return StanceUtils.RemoveStance<FocusStance>(unit);
            yield return StanceUtils.RemoveStance<CalmStance>(unit);
            yield return new ApplyStatusEffectAction<BoostedPowerStance>(unit, 0, 1);
            yield return new ApplyStatusEffectAction<BoostedFocusStance>(unit, 0, 1);
            yield return new ApplyStatusEffectAction<BoostedCalmStance>(unit, 0, 1);
        }
    }
}
