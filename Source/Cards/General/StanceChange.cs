using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using System;
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
            cardConfig.Type = CardType.Skill;
            cardConfig.Cost = new ManaGroup() { Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 0 };
            cardConfig.UpgradedKeywords = Keyword.Initial;
            cardConfig.RelativeCards = new List<string>() { nameof(StanceChangePower), nameof(StanceChangeFocus), nameof(StanceChangeCalm) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(StanceChangePower), nameof(StanceChangeFocus), nameof(StanceChangeCalm) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StanceChangeDef))]
    public sealed class StanceChange:Card
    {
        public override Interaction Precondition()
        {
            Console.WriteLine("ASSSSS");
            var selectList = new List<Card> {};
            if (!base.Battle.Player.HasStatusEffect<PowerStance>())
            {
                selectList.Add(Library.CreateCard<StanceChangePower>());
            }
            if (!base.Battle.Player.HasStatusEffect<FocusStance>())
            {
                selectList.Add(Library.CreateCard<StanceChangeFocus>());
            }
            if (!base.Battle.Player.HasStatusEffect<CalmStance>())
            {
                selectList.Add(Library.CreateCard<StanceChangeCalm>());
            }
            Console.WriteLine(selectList.Count);
            return new SelectCardInteraction(1, 1, selectList);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction interaction))
            {
                yield break;
            }

            foreach (Card card in interaction.SelectedCards)
            {
                OptionCard optionCard = card as OptionCard;
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
}
