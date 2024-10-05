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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 0;
            cardConfig.UpgradedValue1 = 1;
            cardConfig.UpgradedKeywords = Keyword.Retain;
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
        public int RareGain => 3 + Value1;
        public override Interaction Precondition()
        {
            return new MiniSelectCardInteraction(base.Battle.HandZone.Where(c => c is ModFrontlineCard));
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                var c = ((MiniSelectCardInteraction)precondition).SelectedCard as ModFrontlineCard;
                if (c.Config.Rarity == Rarity.Common)
                    yield return new GainHaniwaAction(CommonGain, CommonGain, CommonGain);
                else if (c.Config.Rarity == Rarity.Uncommon)
                    yield return new GainHaniwaAction(UncommonGain, UncommonGain, UncommonGain);
                else if (c.Config.Rarity == Rarity.Rare)
                    yield return new GainHaniwaAction(RareGain, RareGain, RareGain);
                yield return new ExileCardAction(c);
            }
        }
    }
}
