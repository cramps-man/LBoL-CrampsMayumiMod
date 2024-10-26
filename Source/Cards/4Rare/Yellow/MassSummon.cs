using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class MassSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MassSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 3 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Value2 = 2;
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(MassSummonDef))]
    public sealed class MassSummon : Card
    {
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaActionType.Sacrifice, Value2, Value2, Value2);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, Value2, Value2, Value2);

            IEnumerable<Type> possibleSummons = HaniwaFrontlineUtils.AllSummonTypes;
            int emptySlots = base.Battle.MaxHand - base.Battle.HandZone.Count;

            yield return new AddCardsToHandAction(GetRandomCards(emptySlots, possibleSummons));
            yield return new AddCardsToDrawZoneAction(GetRandomCards(Value1, possibleSummons), DrawZoneTarget.Random);
            yield return new AddCardsToDiscardAction(GetRandomCards(Value1, possibleSummons));
        }

        private List<Card> GetRandomCards(int count, IEnumerable<Type> possibleSummons)
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < count; i++)
            {
                Card randomCard = Library.CreateCard(possibleSummons.Sample(base.BattleRng));
                cards.Add(randomCard);
            }
            return cards;
        }
    }
}
