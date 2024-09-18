using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public abstract class ModAssignStatusEffect: StatusEffect
    {
        public int HaniwaAssigned => Duration;
        protected override void OnAdded(Unit unit)
        {
            this.ReactOwnerEvent<CardUsingEventArgs>(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            Count--;
            if (Count == 0)
            {
                this.NotifyActivating();
                foreach (var item in OnAssignmentDone())
                {
                    yield return item;
                    if (base.Battle.BattleShouldEnd)
                        yield break;
                };
                yield return new RemoveStatusEffectAction(this);
            }
        }
        protected abstract IEnumerable<BattleAction> OnAssignmentDone();
    }
}
