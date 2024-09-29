using LBoL.Base;
using LBoL.Base.Extensions;
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
    public sealed class RecycleCreateReservesDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(RecycleCreateReserves);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.Nobody;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.RelativeCards = new List<string>() { nameof(CreateHaniwa) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(CreateHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(RecycleCreateReservesDef))]
    public sealed class RecycleCreateReserves : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int countRemoved = 0;
            foreach (var card in Battle.ExileZone.SampleManyOrAll(Value1, BattleRng))
            {
                yield return new RemoveCardAction(card);
                countRemoved++;
            };
            if (countRemoved > 0)
                yield return new AddCardsToHandAction(Library.CreateCards<CreateHaniwa>(countRemoved));
            else
                yield return new AddCardsToHandAction(Library.CreateCards<CreateHaniwa>(1));
        }
    }
}
