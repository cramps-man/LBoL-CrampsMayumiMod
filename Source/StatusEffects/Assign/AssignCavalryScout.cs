using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using System;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignCavalryScoutDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCavalryScout);
        }
    }

    [EntityLogic(typeof(AssignCavalryScoutDef))]
    public sealed class AssignCavalryScout : ModAssignStatusEffect
    {
        public int TotalDraw => Math.Max(Level / 3, 1);
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield break;
        }

        protected override IEnumerable<BattleAction> BeforeAssignmentDone(bool onTurnStart, int triggerCount)
        {
            yield return new DescriptiveScryAction(new ScryInfo(Level), "Cavalry Scout - Scry");
            yield return new DrawManyCardAction(TotalDraw);
        }
    }
}
