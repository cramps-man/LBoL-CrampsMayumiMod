using LBoL.Core.Battle;
using LBoLMod.GameEvents;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class AssignPauseAction: SimpleAction
    {
        private readonly AssignPauseEventArgs args;

        internal AssignPauseAction(ModAssignStatusEffect statusEffect)
        {
            this.args = new AssignPauseEventArgs
            {
                StatusEffectToPause = statusEffect
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<AssignPauseEventArgs>("AssignStatusPausing", this.args, ModGameEvents.AssignStatusPausing);
            yield return base.CreatePhase("Main", delegate
            {
                args.StatusEffectToPause.IsPaused = true;
                if (args.StatusEffectToPause.CardFencerAssigned > 0)
                    args.StatusEffectToPause.PauseGenFencer++;
                if (args.StatusEffectToPause.CardArcherAssigned > 0)
                    args.StatusEffectToPause.PauseGenArcher++;
                if (args.StatusEffectToPause.CardCavalryAssigned > 0)
                    args.StatusEffectToPause.PauseGenCavalry++;
            });
            yield return base.CreateEventPhase<AssignPauseEventArgs>("AssignStatusPaused", this.args, ModGameEvents.AssignStatusPaused);
        }
    }
}
