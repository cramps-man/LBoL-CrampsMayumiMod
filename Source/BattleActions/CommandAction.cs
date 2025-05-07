using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLMod.GameEvents;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.BattleActions
{
    public sealed class CommandAction: SimpleAction
    {
        private readonly CommandEventArgs args;

        internal CommandAction(List<Card> cardsToCommand, UnitSelector target, bool shouldConsumeLoyalty, string commandSourceName = "")
        {
            this.args = new CommandEventArgs
            {
                CardsToCommand = cardsToCommand,
                Target = target,
                ShouldConsumeLoyalty = shouldConsumeLoyalty,
                CommandSourceName = commandSourceName
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<CommandEventArgs>("Commanding", this.args, ModGameEvents.Commanding);
            yield return base.CreatePhase("Main", delegate
            {
                foreach (var card in args.CardsToCommand)
                {
                    base.React(new Reactor(HaniwaFrontlineUtils.ExecuteOnPlayAction(card, Battle, args.Target, args.ShouldConsumeLoyalty, args.CommandSourceName)), card, ActionCause.Card);
                }
            });
            yield return base.CreateEventPhase<CommandEventArgs>("Commanded", this.args, ModGameEvents.Commanded);
        }
    }
}
