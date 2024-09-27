using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLMod.Exhibits;
using LBoLMod.GameEvents;
using LBoLMod.StatusEffects;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class LoseHaniwaAction: SimpleAction
    {
        private readonly LoseHaniwaEventArgs args;

        internal LoseHaniwaAction(HaniwaActionType actionType, int fencerToLose = 0, int archerToLose = 0, int cavalryToLose = 0)
        {
            this.args = new LoseHaniwaEventArgs
            {
                HaniwaActionType = actionType,
                FencerToLose = fencerToLose,
                ArcherToLose = archerToLose,
                CavalryToLose = cavalryToLose
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
                if (args.FencerToLose > 0)
                    LoseLevel<FencerHaniwa>(args.FencerToLose);
                if (args.ArcherToLose > 0)
                    LoseLevel<ArcherHaniwa>(args.ArcherToLose);
                if (args.CavalryToLose > 0)
                    LoseLevel<CavalryHaniwa>(args.CavalryToLose);
            });
            yield return base.CreateEventPhase<LoseHaniwaEventArgs>("LostHaniwa", this.args, ModGameEvents.LostHaniwa);
            if (this.args.HaniwaActionType == HaniwaActionType.Sacrifice)
                yield return base.CreateEventPhase<LoseHaniwaEventArgs>("SacrificedHaniwa", this.args, ModGameEvents.SacrificedHaniwa);
            if (this.args.HaniwaActionType == HaniwaActionType.Assign)
                yield return base.CreateEventPhase<LoseHaniwaEventArgs>("AssignedHaniwa", this.args, ModGameEvents.AssignedHaniwa);
        }

        private void LoseLevel<T>(int levelToLose) where T: ModHaniwaStatusEffect
        {
            var player = base.Battle.Player;
            if (!player.HasStatusEffect<T>())
                return;
            //move this to an ability, which ill make later
            /*if (args.HaniwaActionType == HaniwaActionType.Sacrifice && player.HasExhibit<ExhibitB>())
                levelToLose = Math.Max(levelToLose - 1, 0);*/

            var se = player.GetStatusEffect<T>();
            if (se.Level > levelToLose)
            {
                se.Level -= levelToLose;
                return;
            }
            base.React(new RemoveStatusEffectAction(se));
        }
    }
}
