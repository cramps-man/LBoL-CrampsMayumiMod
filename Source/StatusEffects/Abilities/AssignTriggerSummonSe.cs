using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignTriggerSummonSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignTriggerSummonSe);
        }
    }

    [EntityLogic(typeof(AssignTriggerSummonSeDef))]
    public sealed class AssignTriggerSummonSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.AssignEffectTriggered, this.OnAssignTriggered);
        }

        private IEnumerable<BattleAction> OnAssignTriggered(AssignTriggerEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            IEnumerable<Type> possibleSummons = HaniwaFrontlineUtils.GetAllSummonTypes(base.Battle);
            List<Card> cards = new List<Card>();
            for (int i = 0; i < Level; i++)
            {
                Card randomCard = Library.CreateCard(possibleSummons.Sample(base.GameRun.BattleRng));
                cards.Add(randomCard);
            }
            if (cards.Count > 0)
            {
                base.NotifyActivating();
                yield return new AddCardsToHandAction(cards);
            }
        }
    }
}
