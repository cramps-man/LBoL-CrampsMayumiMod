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
    public sealed class ArcherPrepVolleyDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ArcherPrepVolley);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Any = 0 };
            cardConfig.Damage = 5;
            cardConfig.Value1 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ArcherPrepVolleyDef))]
    public sealed class ArcherPrepVolley : ModAssignCard
    {
        public override int ArcherAssigned => 1;
        public override int StartingCardCounter => IsUpgraded ? 3 : 5;
        public override int StartingTaskLevel => IsUpgraded ? 10 : 5;
        public override Type AssignStatusType => typeof(AssignArcherPrepVolley);
    }
}
