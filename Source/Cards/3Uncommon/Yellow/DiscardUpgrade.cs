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
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class DiscardUpgradeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DiscardUpgrade);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.Block = 16;
            cardConfig.Value1 = 5;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(DiscardUpgradeDef))]
    public sealed class DiscardUpgrade : Card
    {
        public override bool DiscardCard => true;
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c is ModFrontlineCard).ToList();
            return new SelectHandInteraction(0, list.Count, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            if (!(precondition is SelectHandInteraction discardInteraction))
                yield break;
            if (discardInteraction.SelectedCards.Count == 0)
                yield break;
            for (int i = 0; i < Value1; i++)
                yield return new UpgradeCardsAction(discardInteraction.SelectedCards.Where(c => c is ModFrontlineCard).ToList());
            yield return new DiscardManyAction(discardInteraction.SelectedCards);
        }
    }
}
