using LBoL.Core.Battle;
using LBoLMod.GameEvents;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class AssignTriggerAction: SimpleAction
    {
        private readonly AssignTriggerEventArgs args;

        internal AssignTriggerAction(ModAssignStatusEffect triggeredEffect, bool onTurnStart, bool createTriggeredEvent = true)
        {
            this.args = new AssignTriggerEventArgs
            {
                TriggeredEffect = triggeredEffect,
                OnTurnStart = onTurnStart,
                CreateTriggeredEvent = createTriggeredEvent
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            if (args.CreateTriggeredEvent)
                yield return base.CreateEventPhase<AssignTriggerEventArgs>("AssignEffectTriggering", this.args, ModGameEvents.AssignEffectTriggering);
            yield return base.CreatePhase("Before", delegate
            {
                base.React(new Reactor(args.TriggeredEffect.BeforeAssignmentDone(args.OnTurnStart)), args.TriggeredEffect, ActionCause.StatusEffect);
            });
            yield return base.CreatePhase("Main", delegate
            {
                base.React(new Reactor(args.TriggeredEffect.OnAssignmentDone(args.OnTurnStart)), args.TriggeredEffect, ActionCause.StatusEffect);
            });
            yield return base.CreatePhase("After", delegate
            {
                base.React(new Reactor(args.TriggeredEffect.AfterAssignmentDone(args.OnTurnStart)), args.TriggeredEffect, ActionCause.StatusEffect);
            });
            if (args.CreateTriggeredEvent)
                yield return base.CreateEventPhase<AssignTriggerEventArgs>("AssignEffectTriggered", this.args, ModGameEvents.AssignEffectTriggered);
        }
        public override string ExportDebugDetails()
        {
            return args.ExportDebugDetails();
        }
    }
}
