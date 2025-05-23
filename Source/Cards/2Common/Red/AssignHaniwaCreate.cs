﻿using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignHaniwaCreateDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignHaniwaCreate);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 4;
            cardConfig.Value2 = 3;
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Keywords = Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(AssignmentBonusSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(AssignmentBonusSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignHaniwaCreateDef))]
    public sealed class AssignHaniwaCreate : ModMayumiCard
    {
        public int AssignmentBonusGain => 1;
        public override Interaction Precondition()
        {
            return new SelectCardInteraction(0, 1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player))
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction interaction))
                yield break;
            if (interaction.SelectedCards == null || interaction.SelectedCards.Empty())
            {
                yield return new GainHaniwaAction(1, 1, 1);
                yield break;
            }

            foreach (ModAssignOptionCard optionCard in interaction.SelectedCards)
            {
                optionCard.StatusEffect.Count += Value2;
                int fencerGain = 0;
                int archerGain = 0;
                int cavalryGain = 0;
                if (optionCard.StatusEffect.CardFencerAssigned > 0)
                    fencerGain += Value1;
                if (optionCard.StatusEffect.CardArcherAssigned > 0)
                    archerGain += Value1;
                if (optionCard.StatusEffect.CardCavalryAssigned > 0)
                    cavalryGain += Value1;
                yield return new GainHaniwaAction(fencerGain, archerGain, cavalryGain);
            }
            yield return BuffAction<AssignmentBonusSe>(AssignmentBonusGain);
        }
    }
}
