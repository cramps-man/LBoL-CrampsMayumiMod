using LBoL.Core;
using LBoL.Core.Battle;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public class AssignTriggerEventArgs: GameEventArgs
    {
        public IEnumerable<BattleAction> BattleActions { get; internal set; }
        public IEnumerable<BattleAction> AfterBattleActions { get; internal set; }
        public int TimesToActivate { get; internal set; }
        public bool OnTurnStart { get; internal set; }
    }
}
