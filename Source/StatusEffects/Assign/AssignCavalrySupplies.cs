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
using System.Linq;

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
        public int TotalTimes => Math.Max(CardValue2 + (Level / CardValue1), 2);
        private List<Card> RandomChoices;
        private SelectCardInteraction Interaction { get; set; }
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            RandomChoices = new List<Card>();
            for (int i = 0; i < TotalTimes; i++)
            {
                int rng = base.GameRun.BattleRng.NextInt(1, 100);
                if (rng >= 1 && rng <= 15)
                    RandomChoices.Add(Library.CreateCard<RManaCard>());
                else if (rng >= 16 && rng <= 30)
                    RandomChoices.Add(Library.CreateCard<WManaCard>());
                else if (rng >= 31 && rng <= 35)
                    RandomChoices.Add(Library.CreateCard<PManaCard>());
                else if (rng >= 36 && rng <= 100)
                    RandomChoices.Add(Library.CreateCard(HaniwaFrontlineUtils.GetAllSummonTypes(base.Battle).Sample(base.GameRun.BattleRng)));
            }
            RandomChoices.ForEach(c => c.IsReplenish = true);
            Interaction = new SelectCardInteraction(0, 1, RandomChoices)
            {
                Description = InteractionTitle
            };
            yield return new InteractionAction(Interaction);
        }

        public override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            yield return new AddCardsToDrawZoneAction(Interaction.SelectedCards, DrawZoneTarget.Random);
            yield return new AddCardsToDiscardAction(RandomChoices.Except(Interaction.SelectedCards));
        }
    }
}
