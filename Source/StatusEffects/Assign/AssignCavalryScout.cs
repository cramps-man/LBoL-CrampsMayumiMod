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
        public ScryInfo TotalScry => new ScryInfo(Level / CardValue1);
        public int TotalDraw => Math.Max(Level / CardValue2, 1);
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield return new DescriptiveScryAction(TotalScry, "Cavalry Scout - Scry");
            yield return new DrawManyCardAction(TotalDraw);
        }
    }
}
