using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class FrontlineUpgradeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineUpgrade);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1 };
            cardConfig.Block = 4;
            cardConfig.UpgradedBlock = 6;
            cardConfig.Value1 = 2;
            cardConfig.Value2 = 1;
            cardConfig.UpgradedValue2 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(LoyaltyProtectionSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(LoyaltyProtectionSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineUpgradeDef))]
    public sealed class FrontlineUpgrade : ModMayumiCard
    {
        public int LoyaltyProtectionGain => IsUpgraded ? 6 : 4;
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c != this && c.CanUpgradeAndPositive).ToList();
            return new SelectHandInteraction(0, Value1, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return BuffAction<LoyaltyProtectionSe>(LoyaltyProtectionGain);
            if (!(precondition is SelectHandInteraction selectInteraction))
                yield break;
            if (selectInteraction.SelectedCards.Count == 0)
                yield break;

            yield return new UpgradeCardsAction(selectInteraction.SelectedCards);
            for (int i = 0; i < Value2; i++)
            {
                if (selectInteraction.SelectedCards.Any(c => c.CanUpgradeAndPositive))
                    yield return new UpgradeCardsAction(selectInteraction.SelectedCards.Where(c => c.CanUpgradeAndPositive).ToList());
            }
        }
    }
}
