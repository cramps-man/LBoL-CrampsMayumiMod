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
    public sealed class FrontlineDiscardDrawDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineDiscardDraw);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Blue };
            cardConfig.Cost = new ManaGroup() { Blue = 1, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Blue = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineDiscardDrawDef))]
    public sealed class FrontlineDiscardDraw : Card
    {
        public override bool DiscardCard => true;
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c != this).ToList();
            return new SelectHandInteraction(0, list.Count, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction discardInteraction))
                yield break;

            if (IsUpgraded)
                yield return new UpgradeCardsAction(discardInteraction.SelectedCards.Where(c => c.CanUpgradeAndPositive));
            yield return new DiscardManyAction(discardInteraction.SelectedCards);
            yield return new DrawManyCardAction(discardInteraction.SelectedCards.Where(c => c is ModFrontlineCard).Count());
        }
    }
}
