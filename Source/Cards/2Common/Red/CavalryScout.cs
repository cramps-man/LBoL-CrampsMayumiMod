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
    public sealed class CavalryScoutDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CavalryScout);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Value2 = 3;
            cardConfig.UpgradedValue2 = 1;
            cardConfig.RelativeKeyword = Keyword.Scry;
            cardConfig.UpgradedRelativeKeyword = Keyword.Scry;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalryScoutDef))]
    public sealed class CavalryScout : ModAssignCard
    {
        public override int CavalryAssigned => 1;
        public override int StartingCardCounter => Value2;
        public override int StartingTaskLevel => Value1;
        public override Type AssignStatusType => typeof(AssignCavalryScout);
    }
}
