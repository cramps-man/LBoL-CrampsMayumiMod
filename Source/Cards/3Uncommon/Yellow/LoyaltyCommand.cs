using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class LoyaltyCommandDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LoyaltyCommand);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 8;
            cardConfig.UpgradedValue1 = 13;
            cardConfig.Value2 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(LoyaltyProtectionSe), nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(LoyaltyProtectionSe), nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(LoyaltyCommandDef))]
    public sealed class LoyaltyCommand : Card
    {
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c != this && c is ModFrontlineCard).ToList();
            return new SelectHandInteraction(0, Value2, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<LoyaltyProtectionSe>(Value1);
            if (!(precondition is SelectHandInteraction selectInteraction))
                yield break;

            foreach (var battleAction in HaniwaFrontlineUtils.ExecuteOnPlayActions(selectInteraction.SelectedCards.ToList(), base.Battle, selector, true))
            {
                yield return battleAction;
            }
        }
    }
}
