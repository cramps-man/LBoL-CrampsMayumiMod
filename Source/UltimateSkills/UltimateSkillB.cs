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
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace LBoLMod.UltimateSkills
{
    public sealed class UltimateSkillBDef: UltimateSkillTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(UltimateSkillB);
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
                Damage: 40,
                Value1: 5,
                Value2: 0,
                Keywords: Keyword.Accuracy,
                RelativeEffects: new List<string>() { nameof(Haniwa), nameof(Assign) },
                RelativeCards: new List<string>() { }
                );

            return config;

        }
    }

    [EntityLogic(typeof(UltimateSkillBDef))]
    public sealed class UltimateSkillB : UltimateSkill
    {
        public UltimateSkillB() {
            base.TargetType = TargetType.SingleEnemy;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            var unit = base.Battle.Player;
            yield return HaniwaUtils.RemoveDowntime(unit);
            yield return HaniwaUtils.ForceGainHaniwa<ArcherHaniwa>(unit, Value1);
            yield return HaniwaUtils.ForceGainHaniwa<CavalryHaniwa>(unit, Value1);
            yield return HaniwaUtils.ForceGainHaniwa<FencerHaniwa>(unit, Value1);
            yield return new DamageAction(base.Owner, selector.GetEnemy(base.Battle), Damage);
        }
    }
}
