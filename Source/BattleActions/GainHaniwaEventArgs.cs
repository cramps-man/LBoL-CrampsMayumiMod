using LBoL.Core;
using LBoLMod.Utils;
using System;

namespace LBoLMod.BattleActions
{
    public class GainHaniwaEventArgs: GameEventArgs
    {
        public Type HaniwaType { get; internal set; }
        public int AmountToGain { get; internal set; }
        public bool IsFromAssign { get; internal set; }
    }
}
