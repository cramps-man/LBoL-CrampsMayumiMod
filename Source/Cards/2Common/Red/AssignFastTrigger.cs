using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignFastTriggerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignFastTrigger);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 13;
            cardConfig.UpgradedDamage = 17;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 1;
            cardConfig.Cost = new ManaGroup() { Red = 1, Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentBonusSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentBonusSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignFastTriggerDef))]
    public sealed class AssignFastTrigger : ModMayumiCard
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
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;
            yield return BuffAction<AssignmentBonusSe>(Value2);

            foreach (ModAssignOptionCard card in ((SelectCardInteraction)precondition).SelectedCards)
            {
                card.StatusEffect.Level += card.StatusEffect.Count * Value1;
                foreach (var item in card.StatusEffect.ImmidiatelyTrigger())
                {
                    yield return item;
                }
            }
        }
    }
}
