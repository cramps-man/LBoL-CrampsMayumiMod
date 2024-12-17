using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Cards;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignCostTriggerSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCostTriggerSe);
        }
    }

    [EntityLogic(typeof(AssignCostTriggerSeDef))]
    public sealed class AssignCostTriggerSe: StatusEffect
    {
        public ManaGroup IncreasedCost => ManaGroup.Anys(1);
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(base.Battle.CardUsed, this.OnCardUsed);
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (args.Card is ModAssignCard)
                yield return new GainTurnManaAction(args.ConsumingMana);
        }
    }
}
