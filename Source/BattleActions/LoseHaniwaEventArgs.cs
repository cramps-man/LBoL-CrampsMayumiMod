using LBoL.Core;
using LBoLMod.Utils;

namespace LBoLMod.BattleActions
{
    public class LoseHaniwaEventArgs: GameEventArgs
    {
        public int FencerToLose { get; internal set; }
        public int ArcherToLose { get; internal set; }
        public int CavalryToLose { get; internal set; }
        public HaniwaActionType HaniwaActionType { get; internal set; }
    }
}
