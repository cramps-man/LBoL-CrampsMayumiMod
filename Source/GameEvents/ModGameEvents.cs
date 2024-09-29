using LBoL.Core;
using LBoLMod.BattleActions;

namespace LBoLMod.GameEvents
{
    public sealed class ModGameEvents
    {
        public static GameEvent<AssignTriggerEventArgs> AssignEffectTriggering { get; } = new GameEvent<AssignTriggerEventArgs>();
        public static GameEvent<AssignTriggerEventArgs> AssignEffectTriggered { get; } = new GameEvent<AssignTriggerEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> LosingHaniwa { get; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> LostHaniwa { get; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> SacrificingHaniwa { get; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> SacrificedHaniwa { get; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> AssigningHaniwa { get; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> AssignedHaniwa { get; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<GainHaniwaEventArgs> GainingHaniwa { get; } = new GameEvent<GainHaniwaEventArgs>();
        public static GameEvent<GainHaniwaEventArgs> GainedHaniwa { get; } = new GameEvent<GainHaniwaEventArgs>();
        public static GameEvent<GainHaniwaEventArgs> GainingHaniwaFromAssign { get; } = new GameEvent<GainHaniwaEventArgs>();
        public static GameEvent<GainHaniwaEventArgs> GainedHaniwaFromAssign { get; } = new GameEvent<GainHaniwaEventArgs>();
        public static GameEvent<AssignPauseEventArgs> AssignStatusPausing { get; } = new GameEvent<AssignPauseEventArgs>();
        public static GameEvent<AssignPauseEventArgs> AssignStatusPaused { get; } = new GameEvent<AssignPauseEventArgs>();
    }
}
