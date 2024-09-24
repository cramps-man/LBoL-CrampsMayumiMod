using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLMod.StatusEffects;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.Source.Cards
{
    public abstract class ModAssignCard: Card
    {
        public virtual int HaniwaRequired => 0;
        public virtual Type HaniwaType => null;
        public virtual int StartingCardCounter => 0;
        public virtual Type AssignStatusType => null;
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaType, HaniwaRequired, HaniwaActionType.Assign);
        public override string CantUseMessage => "Need more " + HaniwaNameFromType(HaniwaType);

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return HaniwaUtils.LoseHaniwa(base.Battle.Player, HaniwaType, HaniwaRequired, HaniwaActionType.Assign);
            yield return BuffAction(AssignStatusType, count: StartingCardCounter);
        }

        private static string HaniwaNameFromType(Type haniwaType)
        {
            if (haniwaType == null)
                return "";
            if (haniwaType == typeof(ArcherHaniwa))
                return "Archer";
            if (haniwaType == typeof(CavalryHaniwa))
                return "Cavalry";
            if (haniwaType == typeof(FencerHaniwa))
                return "Fencer";
            return "";
        }
    }
}
