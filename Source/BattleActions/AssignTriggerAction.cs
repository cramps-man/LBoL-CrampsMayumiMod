using LBoL.Core.Battle;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class AssignTriggerAction: SimpleAction
    {
        private readonly AssignTriggerEventArgs args;

        internal AssignTriggerAction(IEnumerable<BattleAction> battleActions, int timesToActivate, bool onTurnStart)
        {
            this.args = new AssignTriggerEventArgs
            {
                BattleActions = battleActions,
                TimesToActivate = timesToActivate,
                OnTurnStart = onTurnStart
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<AssignTriggerEventArgs>("AssignEffectTriggering", this.args, ModGameEvents.AssignEffectTriggering);
            yield return base.CreatePhase("Main", delegate
            {
                for (int i = 0; i < args.TimesToActivate; i++)
                {
                    foreach (var action in args.BattleActions)
                    {
                        if (!base.Battle.BattleShouldEnd)
                            base.React(action);
                    }
                }
            });
            yield return base.CreateEventPhase<AssignTriggerEventArgs>("AssignEffectTriggered", this.args, ModGameEvents.AssignEffectTriggered);
        }
    }
}
