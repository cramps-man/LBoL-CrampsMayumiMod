using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.Source.Cards
{
    public abstract class ModAssignCard: Card
    {
        public virtual int HaniwaRequired => 0;
        public virtual Type HaniwaType => null;
        public virtual int CardsToPlay => 0;
        public virtual Type AssignStatusType => null;
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaType, HaniwaRequired, HaniwaActionType.Assign);
        public override string CantUseMessage => "Need more " + HaniwaType.Name;

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return HaniwaUtils.LoseHaniwa(base.Battle.Player, HaniwaType, HaniwaRequired, HaniwaActionType.Assign);
            yield return BuffAction(AssignStatusType, count: CardsToPlay);
        }
    }
}
