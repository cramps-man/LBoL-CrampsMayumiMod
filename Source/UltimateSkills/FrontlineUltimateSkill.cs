using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.BattleActions;
using LBoLMod.Cards;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace LBoLMod.UltimateSkills
{
    public sealed class FrontlineUltimateSkillDef: UltimateSkillTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineUltimateSkill);
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
                Damage: 0,
                Value1: 5,
                Value2: 3,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { nameof(Haniwa), nameof(Frontline) },
                RelativeCards: new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaUpgrader), nameof(HaniwaExploiter) }
                );

            return config;

        }
    }

    [EntityLogic(typeof(FrontlineUltimateSkillDef))]
    public sealed class FrontlineUltimateSkill : UltimateSkill
    {
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle").RuntimeFormat(this.FormatWrapper);
        public FrontlineUltimateSkill() {
            base.TargetType = TargetType.Self;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return new GainHaniwaAction(Value1, Value1, Value1);
            yield return PerformAction.Wait(0.2f);
            List<ModFrontlineOptionCard> cards = HaniwaFrontlineUtils.GetAllOptionCards(base.Battle, checkSacrificeRequirement: true);
            var selectCardInteraction = new SelectCardInteraction(0, Value2, cards);
            selectCardInteraction.Description = InteractionTitle;
            yield return new InteractionAction(selectCardInteraction);
            foreach (var action in HaniwaFrontlineUtils.CardsSummon(selectCardInteraction.SelectedCards))
            {
                if (action is AddCardsToHandAction addHand)
                {
                    List<Card> beforeList = new List<Card>(addHand.Args.Cards);
                    yield return addHand;
                    List<Card> afterList = new List<Card>(addHand.Args.Cards); //only contains cards added to hand
                    if (beforeList.Count != afterList.Count)
                    {
                        beforeList.RemoveAll(c => afterList.Contains(c));
                        yield return new AddCardsToDrawZoneAction(beforeList, DrawZoneTarget.Random);
                    }
                }
                else
                    yield return action;
            };
        }
    }
}
