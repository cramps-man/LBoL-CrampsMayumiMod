using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Source.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class StanceChangeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StanceChange);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.Self;
            cardConfig.Cost = new ManaGroup() { Any = 0 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 0 };
            cardConfig.Keywords = Keyword.Exile | Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Stance) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Stance) };
            cardConfig.RelativeCards = new List<string>() { nameof(StanceChangePower), nameof(StanceChangeFocus), nameof(StanceChangeCalm) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(StanceChangePower)+"+", nameof(StanceChangeFocus)+"+", nameof(StanceChangeCalm)+"+" };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StanceChangeDef))]
    public sealed class StanceChange:Card
    {
        public override Interaction Precondition()
        {
            var selectList = new List<Card> { Library.CreateCard<StanceChangePower>(IsUpgraded), Library.CreateCard<StanceChangeFocus>(IsUpgraded), Library.CreateCard<StanceChangeCalm>(IsUpgraded) };
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
