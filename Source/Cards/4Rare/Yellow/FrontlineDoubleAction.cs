using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class FrontlineDoubleActionDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineDoubleAction);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2, Any = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.Value1 = 20;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(LoyaltyProtectionSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(LoyaltyProtectionSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineDoubleActionDef))]
    public sealed class FrontlineDoubleAction : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<FrontlineDoubleActionSe>();
            yield return BuffAction<LoyaltyProtectionSe>(Value1);
        }
    }
}
