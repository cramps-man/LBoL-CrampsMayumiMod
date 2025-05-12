using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
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
            cardConfig.Value2 = 2;
            cardConfig.UpgradedValue2 = 3;
            cardConfig.Keywords = Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Retain;
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Command) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Command) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AshesToClayDef))]
    public sealed class AshesToClay : ModMayumiCard
    {
        public override Interaction Precondition()
        {
            List<Card> list = HaniwaFrontlineUtils.GetCommandableCards(base.Battle.ExileZone.ToList(), this);
            return new SelectHandInteraction(0, Value1, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction selectInteraction))
                yield break;

            if (selectInteraction == null || !selectInteraction.SelectedCards.Any())
            {
                foreach (var item in base.Battle.HandZone.Where(c => c is ModFrontlineCard).Cast<ModFrontlineCard>())
                {
                    item.RemainingValue += Value2;
                };
                yield break;
            }

            yield return new CommandAction(selectInteraction.SelectedCards.ToList(), selector, false, Name);
            if (base.Battle.BattleShouldEnd)
                yield break;

            foreach (var card in selectInteraction.SelectedCards)
            {
                yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Random);
            }
        }
    }
}
