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
    public sealed class FreezeFrontlineDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FreezeFrontline);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Blue };
            cardConfig.Cost = new ManaGroup() { Blue = 1 };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.RelativeCards = new List<string>() { nameof(FrozenHaniwa) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(FrozenHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FreezeFrontlineDef))]
    public sealed class FreezeFrontline : Card
    {
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c != this && c is ModFrontlineCard).ToList();
            return new SelectHandInteraction(1, Value1, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction freezeInteraction))
                yield break;

            foreach(ModFrontlineCard card in freezeInteraction.SelectedCards)
            {
                FrozenHaniwa frozen = Library.CreateCard<FrozenHaniwa>();
                frozen.OriginalCard = card;
                frozen.UpgradeCounter = card.UpgradeCounter;
                frozen.RemainingValue = card.RemainingValue;
                yield return new TransformCardAction(card, frozen);
            }
        }
    }
}
