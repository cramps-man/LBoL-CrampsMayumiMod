using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CreateHaniwaDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CreateHaniwa);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.HideMesuem = true;
            cardConfig.Type = CardType.Skill;
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Keywords = Keyword.Exile | Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateHaniwaDef))]
    public sealed class CreateHaniwa : ModMayumiCard
    {
        public override Interaction Precondition()
        {
            var selectList = new List<Card> { Library.CreateCard<CreateHaniwaFencer>(IsUpgraded), Library.CreateCard<CreateHaniwaArcher>(IsUpgraded), Library.CreateCard<CreateHaniwaCavalry>(IsUpgraded) };
            foreach (var card in selectList)
                card.SetBattle(Battle);
            return new MiniSelectCardInteraction(selectList);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is MiniSelectCardInteraction interaction))
            {
                yield break;
            }

            OptionCard optionCard = interaction.SelectedCard as OptionCard;
            if (optionCard != null)
            {
                optionCard.SetBattle(base.Battle);
                foreach (BattleAction battleAction in optionCard.TakeEffectActions())
                {
                    yield return battleAction;
                }
            }
        }
    }
}
