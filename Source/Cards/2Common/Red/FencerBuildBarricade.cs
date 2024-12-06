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
    public sealed class FencerBuildBarricadeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FencerBuildBarricade);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Shield = 8;
            cardConfig.UpgradedShield = 12;
            cardConfig.Value2 = 5;
            cardConfig.UpgradedValue2 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FencerBuildBarricadeDef))]
    public sealed class FencerBuildBarricade : ModAssignCard
    {
        public override int FencerAssigned => 1;
        public override int StartingCardCounter => Value2;
        public override Type AssignStatusType => typeof(AssignFencerBuildBarricade);
    }
}
