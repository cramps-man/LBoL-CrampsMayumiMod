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
    public sealed class FrontlineEnduranceDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineEndurance);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1 };
            cardConfig.Value1 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineEnduranceDef))]
    public sealed class FrontlineEndurance : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card hand) => hand is ModFrontlineCard && hand.IsExile).ToList();
            if (!IsUpgraded)
                return new SelectHandInteraction(0, Value1, list);
            else
                return new SelectHandInteraction(1, list.Count, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            IReadOnlyList<Card> cards = ((SelectHandInteraction)precondition).SelectedCards;
            foreach (ModFrontlineCard card in cards)
            {
                card.IsExile = false;
                card.IsReplenish = true;
            }
            yield return new DrawCardAction();
        }
    }
}
