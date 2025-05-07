using LBoL.Core;

namespace LBoLMod.BattleActions
{
    public class GainHaniwaEventArgs: GameEventArgs
    {
        public int FencerToGain { get; internal set; }
        public int ArcherToGain { get; internal set; }
        public int CavalryToGain { get; internal set; }
        public bool IsFromAssign { get; internal set; }
        public override string GetBaseDebugString()
        {
            return $"FencerGain = [{FencerToGain}], ArcherGain = [{ArcherToGain}], CavalryGain = [{CavalryToGain}], IsFromAssign = [{IsFromAssign}]";
        }
    }
}
