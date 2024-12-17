using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignCavalrySuppliesDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCavalrySupplies);
        }
    }

    [EntityLogic(typeof(AssignCavalrySuppliesDef))]
    public sealed class AssignCavalrySupplies : ModAssignStatusEffect
    {
        public int TotalTimes => Math.Max(Level / CardValue1, 1);
        public int TotalTimesPlusTwo => TotalTimes + 2;
        private SelectCardInteraction Interaction { get; set; }
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            List<Card> randomChoices = new List<Card>();
            for (int i = 0; i < TotalTimes + 2; i++)
            {
                int rng = base.GameRun.BattleRng.NextInt(1, 3);
                if (rng == 1)
                    randomChoices.Add(Library.CreateCard<RManaCard>());
                else if (rng == 2)
                    randomChoices.Add(Library.CreateCard<WManaCard>());
                else if (rng == 3)
                    randomChoices.Add(Library.CreateCard(HaniwaFrontlineUtils.AllSummonTypes.Sample(base.GameRun.BattleRng)));
            }
            Interaction = new SelectCardInteraction(TotalTimes, TotalTimes, randomChoices)
            {
                Description = SourceCardName + " - Choose " + TotalTimes + " to add to draw pile"
            };
            yield return new InteractionAction(Interaction);
        }

        protected override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            yield return new AddCardsToDrawZoneAction(Interaction.SelectedCards, DrawZoneTarget.Random);
        }
    }
}
