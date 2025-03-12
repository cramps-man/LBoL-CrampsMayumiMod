using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.Cards;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class LeadThroughExampleSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LeadThroughExampleSe);
        }
    }

    [EntityLogic(typeof(LeadThroughExampleSeDef))]
    public sealed class LeadThroughExampleSe: StatusEffect
    {
        public int LoyaltyProtectionGain => 5 * Level;
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.AssignEffectTriggered, this.OnAssignEffectTriggered);
            base.ReactOwnerEvent(base.Battle.CardUsed, this.OnCardUsed);
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (args.Card is ModFrontlineCard)
                yield return BuffAction<AssignmentBonusSe>(Level);
        }

        private IEnumerable<BattleAction> OnAssignEffectTriggered(AssignTriggerEventArgs args)
        {
            yield return BuffAction<LoyaltyProtectionSe>(LoyaltyProtectionGain);
        }
    }
}
