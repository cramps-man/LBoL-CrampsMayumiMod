using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.BattleActions
{
    public sealed class DescriptiveScryAction : EventBattleAction<ScryEventArgs>
    {
        private string InteractionTitle = "";
        public DescriptiveScryAction(ScryInfo info, string title = "")
        {
            base.Args = new ScryEventArgs
            {
                ScryInfo = info
            };
            InteractionTitle = title;
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return CreateEventPhase("Scrying", base.Args, base.Battle.Scrying);
            int num = Math.Min(base.Args.ScryInfo.Count, base.Battle.DrawZone.Count);
            if (num <= 0)
            {
                yield break;
            }

            SelectCardInteraction interaction = new SelectCardInteraction(0, num, base.Battle.DrawZone.Take(num), SelectedCardHandling.Keep)
            {
                Source = base.Source,
                Description = InteractionTitle
            };
            yield return CreatePhase("Select", delegate
            {
                React(new InteractionAction(interaction));
            });
            IReadOnlyList<Card> selected = interaction.SelectedCards;
            if (selected.Count > 0)
            {
                yield return CreatePhase("MoveToDiscard", delegate
                {
                    foreach (Card item in selected)
                    {
                        React(new MoveCardAction(item, CardZone.Discard));
                    }
                });
            }

            yield return CreateEventPhase("Scried", base.Args, base.Battle.Scried);
        }
    }
}
