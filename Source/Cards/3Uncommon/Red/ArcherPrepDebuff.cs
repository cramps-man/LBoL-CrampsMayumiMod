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
            cardConfig.Value1 = 25;
            cardConfig.Value2 = 15;
            cardConfig.RelativeKeyword = Keyword.Accuracy;
            cardConfig.UpgradedRelativeKeyword = Keyword.Accuracy;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Vulnerable), nameof(LockedOn) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Vulnerable), nameof(LockedOn) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ArcherPrepDebuffDef))]
    public sealed class ArcherPrepDebuff : ModAssignCard
    {
        public int DivideDmg => 2;
        public override int ArcherAssigned => 2;
        public override int StartingCardCounter => IsUpgraded ? 5 : 7;
        public override int StartingTaskLevel => IsUpgraded ? 20 : 12;
        public override Type AssignStatusType => typeof(AssignArcherPrepDebuff);
    }
}
