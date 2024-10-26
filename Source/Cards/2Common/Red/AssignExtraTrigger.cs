using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignExtraTriggerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignExtraTrigger);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Block = 12;
            cardConfig.UpgradedBlock = 16;
            cardConfig.Cost = new ManaGroup() { Red = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1, Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignExtraTriggerDef))]
    public sealed class AssignExtraTrigger : Card
    {
        public override Interaction Precondition()
        {
            return new SelectCardInteraction(0, 1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player));
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            foreach (ModAssignOptionCard card in ((SelectCardInteraction)precondition).SelectedCards)
            {
                card.StatusEffect.IncreaseExtraTrigger(Value1);
            }
        }
    }
}
