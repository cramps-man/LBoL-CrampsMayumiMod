using LBoL.Core.Battle;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class AssignTriggerAction: SimpleAction
    {
        private readonly AssignTriggerEventArgs args;

        internal AssignTriggerAction(IEnumerable<BattleAction> battleActions, IEnumerable<BattleAction> beforeBattleActions, IEnumerable<BattleAction> afterBattleActions, int taskLevel, bool onTurnStart)
        {
            this.args = new AssignTriggerEventArgs
            {
                BattleActions = battleActions,
                BeforeBattleActions = beforeBattleActions,
                AfterBattleActions = afterBattleActions,
                TaskLevel = taskLevel,
                OnTurnStart = onTurnStart
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<AssignTriggerEventArgs>("AssignEffectTriggering", this.args, ModGameEvents.AssignEffectTriggering);
            yield return base.CreatePhase("Before", delegate
            {
                foreach (var action in args.BeforeBattleActions)
                {
                    if (!base.Battle.BattleShouldEnd)
                        base.React(action);
                }
            });
            yield return base.CreatePhase("Main", delegate
            {
                foreach (var action in args.BattleActions)
                {
                    if (!base.Battle.BattleShouldEnd)
                        base.React(action);
                }
            });
            yield return base.CreatePhase("After", delegate
            {
                foreach (var action in args.AfterBattleActions)
                {
                    if (!base.Battle.BattleShouldEnd)
                        base.React(action);
                }
            });
            yield return base.CreateEventPhase<AssignTriggerEventArgs>("AssignEffectTriggered", this.args, ModGameEvents.AssignEffectTriggered);
        }
    }
}
