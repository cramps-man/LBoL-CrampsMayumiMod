using LBoL.Core;
using LBoLMod.BattleActions;

namespace LBoLMod.GameEvents
{
    public sealed class ModGameEvents
    {
        public static GameEvent<AssignTriggerEventArgs> AssignEffectTriggering { get; set; } = new GameEvent<AssignTriggerEventArgs>();
        public static GameEvent<AssignTriggerEventArgs> AssignEffectTriggered { get; set; } = new GameEvent<AssignTriggerEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> LosingHaniwa { get; set; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> LostHaniwa { get; set; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> SacrificingHaniwa { get; set; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> SacrificedHaniwa { get; set; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> AssigningHaniwa { get; set; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<LoseHaniwaEventArgs> AssignedHaniwa { get; set; } = new GameEvent<LoseHaniwaEventArgs>();
        public static GameEvent<GainHaniwaEventArgs> GainingHaniwa { get; set; } = new GameEvent<GainHaniwaEventArgs>();
        public static GameEvent<GainHaniwaEventArgs> GainedHaniwa { get; set; } = new GameEvent<GainHaniwaEventArgs>();
        public static GameEvent<GainHaniwaEventArgs> GainingHaniwaFromAssign { get; set; } = new GameEvent<GainHaniwaEventArgs>();
        public static GameEvent<GainHaniwaEventArgs> GainedHaniwaFromAssign { get; set; } = new GameEvent<GainHaniwaEventArgs>();
        public static GameEvent<AssignPauseEventArgs> AssignStatusPausing { get; set; } = new GameEvent<AssignPauseEventArgs>();
        public static GameEvent<AssignPauseEventArgs> AssignStatusPaused { get; set; } = new GameEvent<AssignPauseEventArgs>();
        public static GameEvent<ConsumeLoyaltyEventArgs> ConsumingLoyalty { get; set; } = new GameEvent<ConsumeLoyaltyEventArgs>();
        public static GameEvent<ConsumeLoyaltyEventArgs> ConsumedLoyalty { get; set; } = new GameEvent<ConsumeLoyaltyEventArgs>();

        public static void Init()
        {
            AssignEffectTriggering = new GameEvent<AssignTriggerEventArgs>();
            AssignEffectTriggered = new GameEvent<AssignTriggerEventArgs>();
            LosingHaniwa = new GameEvent<LoseHaniwaEventArgs>();
            LostHaniwa = new GameEvent<LoseHaniwaEventArgs>();
            SacrificingHaniwa = new GameEvent<LoseHaniwaEventArgs>();
            SacrificedHaniwa = new GameEvent<LoseHaniwaEventArgs>();
            AssigningHaniwa = new GameEvent<LoseHaniwaEventArgs>();
            AssignedHaniwa = new GameEvent<LoseHaniwaEventArgs>();
            GainingHaniwa = new GameEvent<GainHaniwaEventArgs>();
            GainedHaniwa = new GameEvent<GainHaniwaEventArgs>();
            GainingHaniwaFromAssign = new GameEvent<GainHaniwaEventArgs>();
            GainedHaniwaFromAssign = new GameEvent<GainHaniwaEventArgs>();
            AssignStatusPausing = new GameEvent<AssignPauseEventArgs>();
            AssignStatusPaused = new GameEvent<AssignPauseEventArgs>();
            ConsumingLoyalty = new GameEvent<ConsumeLoyaltyEventArgs>();
            ConsumedLoyalty = new GameEvent<ConsumeLoyaltyEventArgs>();
        }
    }
}
