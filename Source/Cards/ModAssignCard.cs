using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public abstract class ModAssignCard: Card
    {
        public virtual int FencerAssigned => 0;
        public virtual int ArcherAssigned => 0;
        public virtual int CavalryAssigned => 0;
        public virtual int StartingCardCounter => 0;
        public virtual Type AssignStatusType => null;
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaActionType.Assign, FencerAssigned, ArcherAssigned, CavalryAssigned);

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Assign, FencerAssigned, ArcherAssigned, CavalryAssigned);
            yield return BuffAction(AssignStatusType, level: 1, count: StartingCardCounter);
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent(base.Battle.CardsAddedToHand, this.OnCardAddedToHand);
        }

        private void OnCardAddedToHand(CardsEventArgs args)
        {
            SetAssignCostTriggerCost();
        }

        public override IEnumerable<BattleAction> OnDraw()
        {
            SetAssignCostTriggerCost();
            return null;
        }

        public override IEnumerable<BattleAction> OnMove(CardZone srcZone, CardZone dstZone)
        {
            if (dstZone == CardZone.Hand)
                SetAssignCostTriggerCost();
            return null;
        }

        private void SetAssignCostTriggerCost()
        {
            if (base.Battle.Player.HasStatusEffect<AssignCostTriggerSe>())
                base.SetTurnCost(ManaGroup.Anys(1));
        }
    }
}
