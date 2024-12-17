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
    public sealed class AssignUltimateSkillDef: UltimateSkillTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignUltimateSkill);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.ultimateSkillBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("dummyicon.png", BepinexPlugin.directorySource);
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
                RelativeEffects: new List<string>() { nameof(Haniwa), nameof(Assign) },
                RelativeCards: new List<string>() { }
                );

            return config;

        }
    }

    [EntityLogic(typeof(AssignUltimateSkillDef))]
    public sealed class AssignUltimateSkill : UltimateSkill
    {
        public AssignUltimateSkill() {
            base.TargetType = TargetType.AllEnemies;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            var player = base.Battle.Player;
            yield return new DamageAction(player, selector.GetEnemies(base.Battle), Damage);
        }
    }
}
