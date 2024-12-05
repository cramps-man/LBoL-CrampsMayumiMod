using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Helpers;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public abstract class ModAssignCard: Card
    {
        public override string Description => Keywords == Keyword.None ? base.Description + "\n" + UiUtils.WrapByColor(nameof(Assign), GlobalConfig.DefaultKeywordColor) : base.Description + UiUtils.WrapByColor(" " + nameof(Assign), GlobalConfig.DefaultKeywordColor);
        public virtual int FencerAssigned => 0;
        public virtual int ArcherAssigned => 0;
        public virtual int CavalryAssigned => 0;
        public virtual int StartingCardCounter => 0;
        public virtual int StartingTriggers => 1;
        public virtual Type AssignStatusType => null;
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaActionType.Assign, FencerAssigned, ArcherAssigned, CavalryAssigned);
        public override ManaGroup AdditionalCost
        {
            get
            {
                if (base.Battle == null)
                    return ManaGroup.Empty;
                if (base.Battle.HandZone.Contains(this) && base.Battle.Player.TryGetStatusEffect<AssignCostTriggerSe>(out AssignCostTriggerSe s))
                    return ManaGroup.Anys(s.Level);
                return ManaGroup.Empty;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Assign, FencerAssigned, ArcherAssigned, CavalryAssigned);
            yield return BuffAction(AssignStatusType, level: StartingTriggers, count: StartingCardCounter);
        }
    }
}
