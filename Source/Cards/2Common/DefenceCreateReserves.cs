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
    public sealed class DefenceCreateReservesDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DefenceCreateReserves);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            cardConfig.Block = 12;
            cardConfig.UpgradedBlock = 15;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.RelativeCards = new List<string>() { nameof(CreateHaniwa) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(CreateHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(DefenceCreateReservesDef))]
    public sealed class DefenceCreateReserves : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return new AddCardsToHandAction(Library.CreateCards<CreateHaniwa>(Value1));
        }
    }
}
