using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Assign;
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
            cardConfig.Damage = 5;
            cardConfig.UpgradedDamage = 7;
            cardConfig.Block = 2;
            cardConfig.UpgradedBlock = 3;
            cardConfig.Value1 = 9;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FencerPrepCounterDef))]
    public sealed class FencerPrepCounter : ModAssignCard
    {
        public override int FencerAssigned => 2;
        public override int StartingCardCounter => Value1;
        public override Type AssignStatusType => typeof(AssignFencerPrepCounter);
    }
}
