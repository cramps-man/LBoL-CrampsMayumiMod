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
    public sealed class FrontlineCopyDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineCopy);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 2;
            cardConfig.UpgradedValue2 = 3;
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineCopyDef))]
    public sealed class FrontlineCopy : Card
    {
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c is ModFrontlineCard && (c.Config.Rarity == Rarity.Common || c.Config.Rarity == Rarity.Uncommon)).ToList();
            return new SelectHandInteraction(0, Value2, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction copyInteraction))
                yield break;

            List<Card> copiedCards = new List<Card>();
            foreach (ModFrontlineCard card in copyInteraction.SelectedCards)
            {
                var cardClone = (ModFrontlineCard)card.CloneBattleCard();
                cardClone.IsExile = true;
                cardClone.RemainingValue = card.RemainingValue;
                cardClone.IsDarknessMode = card.IsDarknessMode;
                if (card is FrozenHaniwa frozenCard)
                {
                    ((FrozenHaniwa)cardClone).OriginalCard = (ModFrontlineCard)frozenCard.OriginalCard.CloneBattleCard();
                }
                copiedCards.Add(cardClone);
            }
            yield return new AddCardsToDrawZoneAction(copiedCards, DrawZoneTarget.Random);
            yield return new DrawManyCardAction(Value1);
        }
    }
}
