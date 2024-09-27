using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Source.Utils;
using LBoLMod.StatusEffects.Keywords;
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
        public override Interaction Precondition()
        {
            List<Card> cards = new List<Card>();
            foreach (var type in HaniwaFrontlineUtils.CommonSummonTypes)
            {
                var c = Library.CreateCard(type) as ModFrontlineOptionCard;
                c.SetBattle(base.Battle);
                c.NumberToSpawn = Value1;
                if (c.FulfilsRequirement)
                    cards.Add(c);
            }
            return new MiniSelectCardInteraction(cards);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is MiniSelectCardInteraction interaction))
                yield break;

            OptionCard optionCard = interaction.SelectedCard as OptionCard;
            if (optionCard == null)
                yield break;

            foreach (BattleAction battleAction in optionCard.TakeEffectActions())
                yield return battleAction;
        }
    }
}
