using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLMod.Cards;
using LBoLMod.GameEvents;

namespace LBoLMod.BattleActions
{
    public sealed class ConsumeLoyaltyAction: SimpleEventBattleAction<ConsumeLoyaltyEventArgs>
    {
        internal ConsumeLoyaltyAction(ModFrontlineCard frontlineToConsume, int loyaltyConsumption = -1)
        {
            base.Args = new ConsumeLoyaltyEventArgs
            {
                FrontlineToConsume = frontlineToConsume,
                LoyaltyConsumption = loyaltyConsumption,
                TotalConsumption = 0
            };
        }
        public override void PreEventPhase()
        {
            Trigger(ModGameEvents.ConsumingLoyalty);
        }
        public override void MainPhase()
        {
            if (Args.LoyaltyConsumption > 0)
            {
                Args.TotalConsumption += Args.LoyaltyConsumption;
                Args.FrontlineToConsume.RemainingValue -= Args.LoyaltyConsumption;
            }
            if (Args.FrontlineToConsume.RemainingValue < 0)
                base.React(new ExileCardAction(Args.FrontlineToConsume));
        }
        public override void PostEventPhase()
        {
            Trigger(ModGameEvents.ConsumedLoyalty);
        }
    }
}
