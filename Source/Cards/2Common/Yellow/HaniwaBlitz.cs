﻿using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class HaniwaBlitzDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaBlitz);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.Damage = 12;
            cardConfig.UpgradedDamage = 15;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Command) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Command) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaBlitzDef))]
    public sealed class HaniwaBlitz : ModMayumiCard
    {
        public override Interaction Precondition()
        {
            List<Card> list = HaniwaFrontlineUtils.GetCommandableCards(base.Battle.HandZone.ToList(), this);
            return new SelectHandInteraction(0, Value1, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (!(precondition is SelectHandInteraction selectInteraction))
                yield break;

            yield return new CommandAction(selectInteraction.SelectedCards.ToList(), selector, true, Name);
        }
    }
}
