using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLMod.Exhibits;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public abstract class ModAssignStatusEffect: StatusEffect
    {
        public int HaniwaAssigned => Duration;
        protected override void OnAdded(Unit unit)
        {
            this.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
            this.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStarted));
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnEnded, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnEnded));
        }

        private IEnumerable<BattleAction> onPlayerTurnEnded(UnitEventArgs args)
        {
            if (Count <= 3)
                Count = 0;
            else
                Count -= 3;
            yield break;
        }

        private IEnumerable<BattleAction> onPlayerTurnStarted(UnitEventArgs args)
        {
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

        private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
        {
            Count--;
            yield break;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (Count == 0)
            {
                this.NotifyActivating();
                foreach (var item in OnAssignmentDone())
                {
                    yield return item;
                    if (base.Battle.BattleShouldEnd)
                        yield break;
                };
                if (base.Battle.Player.HasExhibit<ExhibitB>())
                {
                    yield return new DrawCardAction();
                }
                yield return new RemoveStatusEffectAction(this);
            }
        }
        protected abstract IEnumerable<BattleAction> OnAssignmentDone();
    }
}
