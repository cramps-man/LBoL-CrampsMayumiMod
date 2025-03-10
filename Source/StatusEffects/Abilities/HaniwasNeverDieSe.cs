using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class HaniwasNeverDieSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwasNeverDieSe);
        }
    }

    [EntityLogic(typeof(HaniwasNeverDieSeDef))]
    public sealed class HaniwasNeverDieSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.SacrificedHaniwa, this.OnSacrificedHaniwa);
        }
        
        private IEnumerable<BattleAction> OnSacrificedHaniwa(LoseHaniwaEventArgs args)
        {
            int fencerToAssign = Math.Min(args.FencerToLose, Level);
            int archerToAssign = Math.Min(args.ArcherToLose, Level);
            int cavalryToAssign = Math.Min(args.CavalryToLose, Level);
            foreach (var battleAction in HaniwaAssignUtils.GetRandomAssignBuffs(base.Battle, fencerToAssign, archerToAssign, cavalryToAssign, false))
            {
                yield return battleAction;
            }
        }
    }
}
