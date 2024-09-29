using LBoL.Core;
using LBoLMod.StatusEffects;

namespace LBoLMod.BattleActions
{
    public class AssignPauseEventArgs: GameEventArgs
    {
        public ModAssignStatusEffect StatusEffectToPause { get; internal set; }
    }
}
