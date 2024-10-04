using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class StrengthInNumbersDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StrengthInNumbers);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 10;
            cardConfig.Mana = new ManaGroup() { Any = 0 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StrengthInNumbersDef))]
    public sealed class StrengthInNumbers : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int zeroCostCount = base.Battle.HandZoneAndPlayArea.Where(c => c.Cost == ManaGroup.Empty).Count();
            if (zeroCostCount <= 0)
                yield break;

            yield return BuffAction<TempFirepower>(Math.Min(zeroCostCount, Value1));
        }
    }
}
