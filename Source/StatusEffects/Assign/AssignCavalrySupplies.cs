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
        public int TotalTimesPlusValue2 => TotalTimes + CardValue2;
        private SelectCardInteraction Interaction { get; set; }
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle").RuntimeFormat(this.FormatWrapper);
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            List<Card> randomChoices = new List<Card>();
            for (int i = 0; i < TotalTimesPlusValue2; i++)
            {
                int rng = base.GameRun.BattleRng.NextInt(1, 100);
                if (rng >= 1 && rng <= 15)
                    randomChoices.Add(Library.CreateCard<RManaCard>());
                else if (rng >= 16 && rng <= 30)
                    randomChoices.Add(Library.CreateCard<WManaCard>());
                else if (rng >= 31 && rng <= 35)
                    randomChoices.Add(Library.CreateCard<PManaCard>());
                else if (rng >= 36 && rng <= 100)
                    randomChoices.Add(Library.CreateCard(HaniwaFrontlineUtils.GetAllSummonTypes(base.Battle).Sample(base.GameRun.BattleRng)));
            }
            randomChoices.ForEach(c => c.IsReplenish = true);
            Interaction = new SelectCardInteraction(TotalTimes, TotalTimes, randomChoices)
            {
                Description = InteractionTitle
            };
            yield return new InteractionAction(Interaction);
        }

        protected override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            yield return new AddCardsToDrawZoneAction(Interaction.SelectedCards, DrawZoneTarget.Random);
        }
    }
}
