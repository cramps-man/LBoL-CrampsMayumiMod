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
    public sealed class AshesToClayDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AshesToClay);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Keywords = Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Retain;
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AshesToClayDef))]
    public sealed class AshesToClay : Card
    {
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.ExileZone.Where(c => c is ModFrontlineCard).ToList();
            return new SelectHandInteraction(1, Value1, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction selectInteraction))
                yield break;

            foreach (var battleAction in HaniwaFrontlineUtils.ExecuteOnPlayActions(selectInteraction.SelectedCards.ToList(), base.Battle, selector))
            {
                yield return battleAction;
            }
            if (base.Battle.BattleShouldEnd)
                yield break;

            foreach (var card in selectInteraction.SelectedCards)
            {
                yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Random);
            }
        }
    }
}
