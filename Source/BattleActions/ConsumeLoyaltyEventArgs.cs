using LBoL.Core;
using LBoLMod.Cards;

namespace LBoLMod.BattleActions
{
    public class ConsumeLoyaltyEventArgs: GameEventArgs
    {
        public ModFrontlineCard FrontlineToConsume { get; internal set; }
        public int LoyaltyConsumption { get; internal set; }
        public int TotalConsumption { get; internal set; }
    }
}
