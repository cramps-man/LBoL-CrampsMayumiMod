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
    public sealed class DefenceSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DefenceSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Any = 1, Red = 1 };
            cardConfig.Block = 10;
            cardConfig.UpgradedBlock = 14;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(DefenceSummonDef))]
    public sealed class DefenceSummon : Card
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
            yield return DefenseAction();

            List<Card> list = new List<Card>();
            summonTypes.Shuffle(base.GameRun.BattleCardRng);
            for (int i = 0; i < base.Value2; i++)
            {
                list.Add(Library.CreateCard(summonTypes[i]));
            }

            SelectCardInteraction interaction = new SelectCardInteraction(1, base.Value1, list)
            {
                Source = this
            };
            yield return new InteractionAction(interaction);
            yield return new AddCardsToHandAction(interaction.SelectedCards);
        }
    }
}
