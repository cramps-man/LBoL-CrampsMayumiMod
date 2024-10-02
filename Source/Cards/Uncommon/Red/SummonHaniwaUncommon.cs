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
using LBoLMod.Utils;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class SummonHaniwaUncommonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(SummonHaniwaUncommon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1, Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(SummonHaniwaUncommonDef))]
    public sealed class SummonHaniwaUncommon : Card
    {
        public override Interaction Precondition()
        {
            List<Card> cards = HaniwaFrontlineUtils.GetUncommonCards(base.Battle, checkSacrificeRequirement: true);
            return new MiniSelectCardInteraction(cards);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is MiniSelectCardInteraction interaction))
                yield break;

            ModFrontlineOptionCard optionCard = interaction.SelectedCard as ModFrontlineOptionCard;
            if (optionCard == null)
                yield break;

            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, optionCard.SelectRequireFencer, optionCard.SelectRequireArcher, optionCard.SelectRequireCavalry);
            yield return new AddCardsToHandAction(optionCard.GetCardsToSpawn());
        }
    }
}
