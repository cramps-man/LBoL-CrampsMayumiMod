using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class SummonCommanderDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(SummonCommander);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 5;
            cardConfig.Cost = new ManaGroup() { Red = 2 };
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaCommander) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaCommander)+"+" };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(SummonCommanderDef))]
    public sealed class SummonCommander : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Card toAdd = Library.CreateCard<HaniwaCommander>();
            if (IsUpgraded)
            {
                toAdd = Library.CreateCard<HaniwaCommander>(IsUpgraded);
                toAdd.UpgradeCounter = Value1;
            }
            yield return new AddCardsToHandAction(toAdd);
        }
    }
}
