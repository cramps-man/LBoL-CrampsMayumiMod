using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLMod.Exhibits;
using LBoLMod.GameEvents;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class LoseHaniwaAction: SimpleAction
    {
        private readonly LoseHaniwaEventArgs args;

        internal LoseHaniwaAction(Type haniwaType, int levelToLose, HaniwaActionType actionType)
        {
            this.args = new LoseHaniwaEventArgs
            {
                HaniwaType = haniwaType,
                AmountToLose = levelToLose,
                HaniwaActionType = actionType
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<LoseHaniwaEventArgs>("LosingHaniwa", this.args, ModGameEvents.LosingHaniwa);
            if (this.args.HaniwaActionType == HaniwaActionType.Sacrifice)
                yield return base.CreateEventPhase<LoseHaniwaEventArgs>("SacrificingHaniwa", this.args, ModGameEvents.SacrificingHaniwa);
            if (this.args.HaniwaActionType == HaniwaActionType.Assign)
                yield return base.CreateEventPhase<LoseHaniwaEventArgs>("AssigningHaniwa", this.args, ModGameEvents.AssigningHaniwa);
            yield return base.CreatePhase("Main", delegate
            {
                var player = base.Battle.Player;
                if (!player.HasStatusEffect(args.HaniwaType))
                    return;
                if (args.HaniwaActionType == HaniwaActionType.Sacrifice && player.HasExhibit<ExhibitB>())
                    args.AmountToLose = Math.Max(args.AmountToLose - 1, 0);

                var se = player.GetStatusEffect(args.HaniwaType);
                if (se.Level > args.AmountToLose)
                {
                    se.Level -= args.AmountToLose;
                    return;
                }
                base.React(new RemoveStatusEffectAction(se));
            });
            yield return base.CreateEventPhase<LoseHaniwaEventArgs>("LostHaniwa", this.args, ModGameEvents.LostHaniwa);
            if (this.args.HaniwaActionType == HaniwaActionType.Sacrifice)
                yield return base.CreateEventPhase<LoseHaniwaEventArgs>("SacrificedHaniwa", this.args, ModGameEvents.SacrificedHaniwa);
            if (this.args.HaniwaActionType == HaniwaActionType.Assign)
                yield return base.CreateEventPhase<LoseHaniwaEventArgs>("AssignedHaniwa", this.args, ModGameEvents.AssignedHaniwa);
        }
    }
}
