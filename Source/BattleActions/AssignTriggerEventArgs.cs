using LBoL.Core;
using LBoLMod.StatusEffects;

namespace LBoLMod.BattleActions
{
    public class AssignTriggerEventArgs: GameEventArgs
    {
        public ModAssignStatusEffect TriggeredEffect { get; internal set; }
        public bool OnTurnStart { get; internal set; }
        public bool CreateTriggeredEvent { get; internal set; }

        public override string GetBaseDebugString()
        {
            return $"TriggerBuff = [{TriggeredEffect.Name}], TurnStart = [{OnTurnStart}], CreateTriggerEvent = [{CreateTriggeredEvent}]";
        }
    }
}
