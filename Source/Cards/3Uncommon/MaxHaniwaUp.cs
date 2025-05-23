﻿using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class MaxHaniwaUpDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MaxHaniwaUp);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Cost = new ManaGroup() { White = 1, Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Hybrid = 1, HybridColor = 2, Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(MaxHaniwaUpDef))]
    public sealed class MaxHaniwaUp : ModMayumiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<MaxHaniwaUpSe>(Value1);
            yield return new GainHaniwaAction(Value1, Value1, Value1);
        }
    }
}
