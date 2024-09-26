using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class SummonHaniwaDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(SummonHaniwa);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Value2 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(SummonHaniwaDef))]
    public sealed class SummonHaniwa : Card
    {
        public readonly List<Type> summonTypes = new List<Type>
        {
            typeof(HaniwaBodyguard),
            typeof(SupportFront),
            typeof(HaniwaAttacker),
            typeof(HaniwaDistraction)
        };
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Card> list = new List<Card>();
            summonTypes.Shuffle(base.GameRun.BattleCardRng);
            for (int i = 0; i < summonTypes.Count; i++)
            {
                list.Add(Library.CreateCard(summonTypes[i]));
            }

            SelectCardInteraction interaction = new SelectCardInteraction(Value2, Value2, list)
            {
                Source = this
            };
            yield return new InteractionAction(interaction);
            yield return new AddCardsToHandAction(interaction.SelectedCards[0].Clone(Value1));
        }
    }
}
