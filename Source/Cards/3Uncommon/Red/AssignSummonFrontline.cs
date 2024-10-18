using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class AssignSummonFrontlineDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignSummonFrontline);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignSummonFrontlineDef))]
    public sealed class AssignSummonFrontline : Card
    {
        public override Interaction Precondition()
        {
            List<Card> assignBuffs = HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player);
            return new SelectCardInteraction(1, 1, assignBuffs);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction assignInteraction))
                yield break;

            var summonInteraction = new SelectCardInteraction(1, 1, HaniwaFrontlineUtils.AllSummonTypes.ConvertAll(t => Library.CreateCard(t)));
            yield return new InteractionAction(summonInteraction);

            foreach (ModAssignOptionCard optionCard in assignInteraction.SelectedCards)
            {
                yield return new RemoveStatusEffectAction(optionCard.StatusEffect);
            }
            List<Card> cards = new List<Card>();
            for (int i = 0; i < Value1; i++)
            {
                cards.Add(summonInteraction.SelectedCards.FirstOrDefault().CloneBattleCard());
            }
            yield return new AddCardsToHandAction(cards);
        }
    }
}
