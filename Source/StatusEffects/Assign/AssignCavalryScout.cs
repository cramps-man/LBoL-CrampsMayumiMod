using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Abilities;
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
        public int TotalAssignmentBonus => Math.Max(Level / CardValue2, 1);
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield return new DescriptiveScryAction(TotalScry, InteractionTitle);
            yield return BuffAction<AssignmentBonusSe>(TotalAssignmentBonus);
            if (onTurnStart)
                yield return new DrawCardAction();
        }
    }
}
