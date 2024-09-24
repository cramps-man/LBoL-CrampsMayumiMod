using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLMod.GameEvents;
using System;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class GainHaniwaAction: SimpleAction
    {
        private readonly GainHaniwaEventArgs args;

        internal GainHaniwaAction(Type haniwaType, int amountToGain, bool isFromAssign = false)
        {
            this.args = new GainHaniwaEventArgs
            {
                HaniwaType = haniwaType,
                AmountToGain = amountToGain,
                IsFromAssign = isFromAssign
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<GainHaniwaEventArgs>("GainingHaniwa", this.args, ModGameEvents.GainingHaniwa);
            if (this.args.IsFromAssign)
                yield return base.CreateEventPhase<GainHaniwaEventArgs>("GainingHaniwaFromAssign", this.args, ModGameEvents.GainingHaniwaFromAssign);
            yield return base.CreatePhase("Main", delegate
            {
                base.React(new ApplyStatusEffectAction(args.HaniwaType, base.Battle.Player, args.AmountToGain));
            });
            yield return base.CreateEventPhase<GainHaniwaEventArgs>("GainedHaniwa", this.args, ModGameEvents.GainedHaniwa);
            if (this.args.IsFromAssign)
                yield return base.CreateEventPhase<GainHaniwaEventArgs>("GainedHaniwaFromAssign", this.args, ModGameEvents.GainedHaniwaFromAssign);
        }
    }
}
