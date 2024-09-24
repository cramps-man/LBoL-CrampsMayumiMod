using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Source.Cards;
using LBoLMod.StatusEffects;
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
            cardConfig.TargetType = TargetType.Nobody;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Any = 0 };
            cardConfig.Shield = 12;
            cardConfig.UpgradedShield = 16;
            cardConfig.Value2 = 9;
            cardConfig.UpgradedValue2 = 5;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FencerBuildBarricadeDef))]
    public sealed class FencerBuildBarricade : ModAssignCard
    {
        public override int HaniwaRequired => 2;
        public override Type HaniwaType => typeof(FencerHaniwa);
        public override int CardsToPlay => Value2;
        public override Type AssignStatusType => typeof(AssignFencerBuildBarricade);
    }
}
