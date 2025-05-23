﻿using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Assign;
using LBoLMod.StatusEffects.Debuffs;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class FencerPrepCounterDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FencerPrepCounter);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 8;
            cardConfig.Block = 2;
            cardConfig.UpgradedBlock = 3;
            cardConfig.Value1 = 6;
            cardConfig.UpgradedValue1 = 8;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(AssignmentMarkSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(AssignmentMarkSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FencerPrepCounterDef))]
    public sealed class FencerPrepCounter : ModAssignCard
    {
        public override int FencerAssigned => 2;
        public override int StartingCardCounter => 9;
        public override int StartingTaskLevel => IsUpgraded ? 16 : 10;
        public override Type AssignStatusType => typeof(AssignFencerPrepCounter);
    }
}
