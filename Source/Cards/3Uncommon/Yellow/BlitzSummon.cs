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
    public sealed class BlitzSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BlitzSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2 };
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 4;
            cardConfig.UpgradedValue2 = 14;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BlitzSummonDef))]
    public sealed class BlitzSummon : Card
    {
        public override Interaction Precondition()
        {
            List<Card> cards = HaniwaFrontlineUtils.GetAllOptionCards(base.Battle, checkSacrificeRequirement: true);
            return new SelectCardInteraction(1, Value1, cards);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (var action in HaniwaFrontlineUtils.CardsSummon(((SelectCardInteraction)precondition).SelectedCards, startingLoyalty: Value2))
            {
                yield return action;
                if (action is AddCardsToHandAction addCardsAction)
                {
                    foreach (var battleAction in HaniwaFrontlineUtils.ExecuteOnPlayActions(addCardsAction.Args.Cards.ToList(), base.Battle, selector, true))
                    {
                        yield return battleAction;
                    }
                }
            };
        }
    }
}
