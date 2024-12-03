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
            cardConfig.Cost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Value2 = 1;
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
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.ExileZone.Where(c => c is ModFrontlineCard && !(c is HaniwaCommander)).ToList();
            return new SelectHandInteraction(0, Value1, list);
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
                yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Top);
            }
            yield return new DrawManyCardAction(Value2);
        }
    }
}
