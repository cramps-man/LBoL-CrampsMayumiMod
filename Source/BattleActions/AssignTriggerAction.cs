using LBoL.Core.Battle;
using LBoLMod.GameEvents;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class AssignTriggerAction: SimpleAction
    {
        private readonly AssignTriggerEventArgs args;

        internal AssignTriggerAction(ModAssignStatusEffect triggeredEffect, bool onTurnStart)
        {
            this.args = new AssignTriggerEventArgs
            {
                TriggeredEffect = triggeredEffect,
                OnTurnStart = onTurnStart
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<AssignTriggerEventArgs>("AssignEffectTriggering", this.args, ModGameEvents.AssignEffectTriggering);
            yield return base.CreatePhase("Before", delegate
            {
                foreach (var action in args.TriggeredEffect.BeforeAssignmentDone(args.OnTurnStart))
                {
                    if (!base.Battle.BattleShouldEnd)
                        base.React(action);
                }
            });
            yield return base.CreatePhase("Main", delegate
            {
                foreach (var action in args.TriggeredEffect.OnAssignmentDone(args.OnTurnStart))
                {
                    if (!base.Battle.BattleShouldEnd)
                        base.React(action);
                }
            });
            yield return base.CreatePhase("After", delegate
            {
                foreach (var action in args.TriggeredEffect.AfterAssignmentDone(args.OnTurnStart))
                {
                    if (!base.Battle.BattleShouldEnd)
                        base.React(action);
                }
            });
            yield return base.CreateEventPhase<AssignTriggerEventArgs>("AssignEffectTriggered", this.args, ModGameEvents.AssignEffectTriggered);
        }
    }
}
