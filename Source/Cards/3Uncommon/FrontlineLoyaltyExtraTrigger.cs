using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class FrontlineLoyaltyExtraTriggerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineLoyaltyExtraTrigger);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 12;
            cardConfig.UpgradedValue2 = 10;
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
            cardConfig.Keywords = Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineLoyaltyExtraTriggerDef))]
    public sealed class FrontlineLoyaltyExtraTrigger : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c is ModFrontlineCard).ToList();
            return new SelectHandInteraction(1, Value1, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction frontlineInteraction))
                yield break;

            var assignBuffs = HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player);
            if (assignBuffs.Count > 0)
            {
                var assignInteraction = new SelectCardInteraction(1, 1, assignBuffs);
                yield return new InteractionAction(assignInteraction);
                foreach (ModAssignOptionCard optionCard in assignInteraction.SelectedCards)
                {
                    optionCard.StatusEffect.NotifyActivating();
                    int totalLoyalty = frontlineInteraction.SelectedCards.Cast<ModFrontlineCard>().Sum(c => c.RemainingValue);
                    optionCard.StatusEffect.IncreaseExtraTrigger(1 + totalLoyalty / Value2);
                }
                yield return PerformAction.Wait(0.3f);
                foreach (ModFrontlineCard card in frontlineInteraction.SelectedCards)
                {
                    card.NotifyActivating();
                    card.ConsumeLoyalty(card.RemainingValue);
                }
            }
        }
    }
}
