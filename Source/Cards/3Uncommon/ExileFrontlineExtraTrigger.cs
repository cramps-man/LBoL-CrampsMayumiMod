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
    public sealed class ExileFrontlineExtraTriggerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ExileFrontlineExtraTrigger);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 1;
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, Any = 1, HybridColor = 2 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ExileFrontlineExtraTriggerDef))]
    public sealed class ExileFrontlineExtraTrigger : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c is ModFrontlineCard).ToList();
            return new SelectHandInteraction(1, Value1, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction exileInteraction))
                yield break;

            int exileCount = exileInteraction.SelectedCards.Count;
            var assignBuffs = HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player);
            if (assignBuffs.Count > 0)
            {
                var assignInteraction = new SelectCardInteraction(1, exileCount, assignBuffs);
                yield return new InteractionAction(assignInteraction);
                foreach (ModAssignOptionCard optionCard in assignInteraction.SelectedCards)
                {
                    optionCard.StatusEffect.NotifyActivating();
                    optionCard.StatusEffect.Level += Value2;
                }
            }

            yield return new ExileManyCardAction(exileInteraction.SelectedCards);
        }
    }
}
