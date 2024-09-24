using LBoL.Core;
using LBoLMod.Utils;
using System;

namespace LBoLMod.BattleActions
{
    public class LoseHaniwaEventArgs: GameEventArgs
    {
        public Type HaniwaType { get; internal set; }
        public int AmountToLose { get; internal set; }
        public HaniwaActionType HaniwaActionType { get; internal set; }
    }
}
