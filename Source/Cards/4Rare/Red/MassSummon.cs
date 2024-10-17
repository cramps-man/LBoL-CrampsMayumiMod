using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 3 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(MassSummonDef))]
    public sealed class MassSummon : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            IEnumerable<Type> possibleSummons = HaniwaFrontlineUtils.CommonSummonTypes.Concat(HaniwaFrontlineUtils.UncommonSummonTypes);
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
