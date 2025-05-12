using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class EnhancedTrainingDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(EnhancedTraining);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Value2 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(EnhancedTrainingDef))]
    public sealed class EnhancedTraining : ModMayumiCard
    {
        public override Interaction Precondition()
        {
            var cards = base.Battle.HandZone.Where(c => c is ModFrontlineCard);
            return new SelectHandInteraction(0, 1, cards)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<EnhancedTrainingSe>(level: Value2);
            if (!(precondition is SelectHandInteraction upgradeInteraction))
                yield break;
            if (upgradeInteraction.SelectedCards == null)
                yield break;
            if (upgradeInteraction.SelectedCards.Count == 0)
                yield break;
            for (int i = 0; i < Value1; i++)
                yield return new UpgradeCardsAction(upgradeInteraction.SelectedCards.Where(c => c is ModFrontlineCard).ToList());
        }
    }
}
