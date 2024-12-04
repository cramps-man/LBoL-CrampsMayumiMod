using LBoL.Base;
using LBoL.Base.Extensions;
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
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class FrontlineHaniwaCreateDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineHaniwaCreate);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 1;
            cardConfig.Cost = new ManaGroup() { White = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Keywords = Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineHaniwaCreateDef))]
    public sealed class FrontlineHaniwaCreate : Card
    {
        public int CommonGain => 1 + Value1;
        public int UncommonGain => 2 + Value1;
        public int RareGain => 4 + Value1;
        public override Interaction Precondition()
        {
            return new SelectCardInteraction(0, 1, base.Battle.HandZone.Where(c => c is ModFrontlineCard));
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (((SelectCardInteraction)precondition).SelectedCards.Empty())
            {
                yield return new GainHaniwaAction(Value2, Value2, Value2);
                yield break;
            }

            foreach (Card card in ((SelectCardInteraction)precondition).SelectedCards)
            {
                if (card.Config.Rarity == Rarity.Common)
                    yield return new GainHaniwaAction(CommonGain, CommonGain, CommonGain);
                else if (card.Config.Rarity == Rarity.Uncommon)
                    yield return new GainHaniwaAction(UncommonGain, UncommonGain, UncommonGain);
                else if (card.Config.Rarity == Rarity.Rare)
                    yield return new GainHaniwaAction(RareGain, RareGain, RareGain);
                yield return new ExileCardAction(card);
            }
        }
    }
}
