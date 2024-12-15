using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
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
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            List<Card> toAdd = new List<Card>();
            for (int i = 0; i < TotalTimes; i++)
            {
                if (i % 2 == 0)
                    toAdd.Add(Library.CreateCard<RManaCard>());
                else
                    toAdd.Add(Library.CreateCard<WManaCard>());
            }
            yield return new AddCardsToDrawZoneAction(toAdd, DrawZoneTarget.Random);
        }
    }
}
