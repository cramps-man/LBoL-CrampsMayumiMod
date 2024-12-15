using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Assign;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class ArcherPrepDebuffDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ArcherPrepDebuff);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 1;
            cardConfig.Value1 = 12;
            cardConfig.UpgradedValue1 = 10;
            cardConfig.RelativeKeyword = Keyword.Accuracy;
            cardConfig.UpgradedRelativeKeyword = Keyword.Accuracy;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Vulnerable) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Vulnerable) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ArcherPrepDebuffDef))]
    public sealed class ArcherPrepDebuff : ModAssignCard
    {
        public override int ArcherAssigned => 2;
        public override int StartingCardCounter => IsUpgraded ? 5 : 7;
        public override int StartingTaskLevel => IsUpgraded ? 10 : 5;
        public override Type AssignStatusType => typeof(AssignArcherPrepDebuff);
    }
}
