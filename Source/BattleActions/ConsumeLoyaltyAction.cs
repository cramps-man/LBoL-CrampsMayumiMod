using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLMod.Cards;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class ConsumeLoyaltyAction: SimpleAction
    {
        private readonly ConsumeLoyaltyEventArgs args;

        internal ConsumeLoyaltyAction(ModFrontlineCard frontlineToConsume, int loyaltyConsumption = -1)
        {
            this.args = new ConsumeLoyaltyEventArgs
            {
                FrontlineToConsume = frontlineToConsume,
                LoyaltyConsumption = loyaltyConsumption,
                TotalConsumption = 0
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<ConsumeLoyaltyEventArgs>("FrontlineLoyaltyConsuming", this.args, ModGameEvents.ConsumingLoyalty);
            yield return base.CreatePhase("Main", delegate
            {
                if (args.LoyaltyConsumption > 0)
                {
                    args.TotalConsumption += args.LoyaltyConsumption;
                    args.FrontlineToConsume.RemainingValue -= args.LoyaltyConsumption;
                }
                if (args.FrontlineToConsume.RemainingValue < 0)
                    base.React(new ExileCardAction(args.FrontlineToConsume));
            });
            yield return base.CreateEventPhase<ConsumeLoyaltyEventArgs>("FrontlineLoyaltyConsumed", this.args, ModGameEvents.ConsumedLoyalty);
        }
    }
}
