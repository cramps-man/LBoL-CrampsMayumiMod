using LBoL.Core;
using LBoLMod.BattleActions;

namespace LBoLMod.GameEvents
{
    public sealed class ModGameEvents
    {
        public static GameEvent<AssignTriggerEventArgs> AssignEffectTriggering { get; } = new GameEvent<AssignTriggerEventArgs>();
        public static GameEvent<AssignTriggerEventArgs> AssignEffectTriggered { get; } = new GameEvent<AssignTriggerEventArgs>();
    }
}
