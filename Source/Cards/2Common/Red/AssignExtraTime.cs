using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignExtraTimeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignExtraTime);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Block = 12;
            cardConfig.UpgradedBlock = 16;
            cardConfig.Cost = new ManaGroup() { Red = 1, Any = 1 };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Value2 = 8;
            cardConfig.UpgradedValue2 = 14;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.RelativeCards = new List<string>() { nameof(AssignmentOrder) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(AssignmentOrder) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignExtraTimeDef))]
    public sealed class AssignExtraTime : ModMayumiCard
    {
        public override Interaction Precondition()
        {
            return new SelectCardInteraction(0, 1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player))
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return new AddCardsToHandAction(Library.CreateCard<AssignmentOrder>());
            foreach (ModAssignOptionCard card in ((SelectCardInteraction)precondition).SelectedCards)
            {
                card.StatusEffect.Count += Value1;
                card.StatusEffect.Level += Value2;
            }
        }
    }
}
