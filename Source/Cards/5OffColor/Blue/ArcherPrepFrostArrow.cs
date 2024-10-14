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
    public sealed class ArcherPrepFrostArrowDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ArcherPrepFrostArrow);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Blue };
            cardConfig.Cost = new ManaGroup() { Any = 0 };
            cardConfig.Damage = 15;
            cardConfig.UpgradedDamage = 20;
            cardConfig.Block = 8;
            cardConfig.UpgradedBlock = 12;
            cardConfig.Value2 = 6;
            cardConfig.UpgradedValue2 = 4;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ArcherPrepFrostArrowDef))]
    public sealed class ArcherPrepFrostArrow : ModAssignCard
    {
        public override int ArcherAssigned => 1;
        public override int StartingCardCounter => Value2;
        public override Type AssignStatusType => typeof(AssignArcherPrepFrostArrow);
    }
}
