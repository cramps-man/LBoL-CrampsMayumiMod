using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLMod.GameEvents;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class GainHaniwaAction: SimpleAction
    {
        private readonly GainHaniwaEventArgs args;

        internal GainHaniwaAction(int fencerToGain = 0, int archerToGain = 0, int cavalryToGain = 0, bool isFromAssign = false)
        {
            this.args = new GainHaniwaEventArgs
            {
                FencerToGain = fencerToGain,
                ArcherToGain = archerToGain,
                CavalryToGain = cavalryToGain,
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
                if (args.FencerToGain > 0)
                    base.React(new ApplyStatusEffectAction<FencerHaniwa>(base.Battle.Player, args.FencerToGain));
                if (args.ArcherToGain > 0)
                    base.React(new ApplyStatusEffectAction<ArcherHaniwa>(base.Battle.Player, args.ArcherToGain));
                if (args.CavalryToGain > 0)
                    base.React(new ApplyStatusEffectAction<CavalryHaniwa>(base.Battle.Player, args.CavalryToGain));
            });
            yield return base.CreateEventPhase<GainHaniwaEventArgs>("GainedHaniwa", this.args, ModGameEvents.GainedHaniwa);
            if (this.args.IsFromAssign)
                yield return base.CreateEventPhase<GainHaniwaEventArgs>("GainedHaniwaFromAssign", this.args, ModGameEvents.GainedHaniwaFromAssign);
        }
    }
}
