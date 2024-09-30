using LBoL.Core.Battle;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class AssignTriggerAction: SimpleAction
    {
        private readonly AssignTriggerEventArgs args;

        internal AssignTriggerAction(IEnumerable<BattleAction> battleActions)
        {
            this.args = new AssignTriggerEventArgs
            {
                BattleActions = battleActions
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<AssignTriggerEventArgs>("AssignEffectTriggering", this.args, ModGameEvents.AssignEffectTriggering);
            yield return base.CreatePhase("Main", delegate
            {
                foreach (var action in args.BattleActions)
                {
                    if (!base.Battle.BattleShouldEnd)
                        base.React(action);
                }
            });
            yield return base.CreateEventPhase<AssignTriggerEventArgs>("AssignEffectTriggered", this.args, ModGameEvents.AssignEffectTriggered);
        }
    }
}
