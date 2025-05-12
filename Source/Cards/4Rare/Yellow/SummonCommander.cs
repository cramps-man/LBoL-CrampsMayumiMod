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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Value1 = 5;
            cardConfig.Value2 = 5;
            cardConfig.Cost = new ManaGroup() { White = 2 };
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.RelativeCards = new List<string>() { nameof(FrontlineCommander), nameof(AssignCommander) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(FrontlineCommander)+"+", nameof(AssignCommander)+"+" };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(SummonCommanderDef))]
    public sealed class SummonCommander : ModMayumiCard
    {
        public override Interaction Precondition()
        {
            var toSelect = new List<Card>() { Library.CreateCard<FrontlineCommander>(), Library.CreateCard<AssignCommander>() };
            return new MiniSelectCardInteraction(toSelect)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is MiniSelectCardInteraction selectInteraction))
                yield break;
            Card toAdd = selectInteraction.SelectedCard;
            if (IsUpgraded)
                toAdd.UpgradeCounter = Value1;
            yield return new AddCardsToHandAction(toAdd);
            if (IsUpgraded)
                foreach (var frontline in Battle.HandZone.Where(c => c is ModFrontlineCard).Cast<ModFrontlineCard>())
                {
                    frontline.RemainingValue += Value2;
                };
        }
    }
}
