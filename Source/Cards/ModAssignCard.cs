using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLMod.BattleActions;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public abstract class ModAssignCard: Card
    {
        public virtual int FencerRequired => 0;
        public virtual int ArcherRequired => 0;
        public virtual int CavalryRequired => 0;
        public virtual int StartingCardCounter => 0;
        public virtual Type AssignStatusType => null;
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaActionType.Assign, FencerRequired, ArcherRequired, CavalryRequired);

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Assign, FencerRequired, ArcherRequired, CavalryRequired);
            yield return BuffAction(AssignStatusType, count: StartingCardCounter);
        }
    }
}
