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
    public abstract class ModAssignCard : Card
    {
        public override string Description => Keywords == Keyword.None ? base.Description + "\n" + UiUtils.WrapByColor(nameof(Assign), GlobalConfig.DefaultKeywordColor) : base.Description + UiUtils.WrapByColor(" " + nameof(Assign), GlobalConfig.DefaultKeywordColor);
        public virtual int FencerAssigned => 0;
        public virtual int ArcherAssigned => 0;
        public virtual int CavalryAssigned => 0;
        public int FencerRequired
        {
            get
            {
                if (base.Battle == null)
                    return FencerAssigned;
                if (base.Battle.Player.HasStatusEffect(AssignStatusType))
                    return Math.Min(FencerAssigned, 1);
                return FencerAssigned;
            }
        }
        public int ArcherRequired
        {
            get
            {
                if (base.Battle == null)
                    return ArcherAssigned;
                if (base.Battle.Player.HasStatusEffect(AssignStatusType))
                    return Math.Min(ArcherAssigned, 1);
                return ArcherAssigned;
            }
        }
        public int CavalryRequired
        {
            get
            {
                if (base.Battle == null)
                    return CavalryAssigned;
                if (base.Battle.Player.HasStatusEffect(AssignStatusType))
                    return Math.Min(CavalryAssigned, 1);
                return CavalryAssigned;
            }
        }
        public virtual int StartingCardCounter => 0;
        public virtual int StartingTaskLevel => 1;
        public virtual Type AssignStatusType => null;
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaActionType.Assign, FencerRequired, ArcherRequired, CavalryRequired);
        public override ManaGroup AdditionalCost
        {
            get
            {
                if (base.Battle == null)
                    return ManaGroup.Empty;
                if (base.Battle.HandZone.Contains(this) && base.Battle.Player.HasStatusEffect<AssignCostTriggerSe>())
                    return ManaGroup.Anys(1);
                return ManaGroup.Empty;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Assign, FencerRequired, ArcherRequired, CavalryRequired);
            yield return BuffAction(AssignStatusType, level: StartingTaskLevel, count: StartingCardCounter);
        }
    }
}
