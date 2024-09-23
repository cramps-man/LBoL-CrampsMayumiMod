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
using LBoLMod.StatusEffects.Keywords;
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
                Damage: 25,
                Value1: 0,
                Value2: 0,
                Keywords: Keyword.Accuracy,
                RelativeEffects: new List<string>() { nameof(Haniwa) },
                RelativeCards: new List<string>() { }
                );

            return config;

        }
    }

    [EntityLogic(typeof(UltimateSkillADef))]
    public sealed class UltimateSkillA : UltimateSkill
    {
        public UltimateSkillA() {
            base.TargetType = TargetType.AllEnemies;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            var player = base.Battle.Player;
            yield return new DamageAction(player, selector.GetEnemies(base.Battle), Damage);
        }
    }
}
