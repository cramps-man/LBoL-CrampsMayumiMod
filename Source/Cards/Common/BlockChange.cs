using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class BlockChangeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BlockChange);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Green };
            cardConfig.Block = 12;
            cardConfig.UpgradedBlock = 16;
            cardConfig.Cost = new ManaGroup() { Any = 1, Green = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1, Green = 1 };
            cardConfig.RelativeCards = new List<string>() { nameof(StanceChange) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(StanceChange)+"+" };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BlockChangeDef))]
    public sealed class BlockChange : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<StanceChange>(IsUpgraded) });
        }
    }
}
