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
    public sealed class AttackCreateReservesDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AttackCreateReserves);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Damage = 10;
            cardConfig.UpgradedDamage = 15;
            cardConfig.Value1 = 1;
            cardConfig.RelativeCards = new List<string>() { nameof(CreateHaniwa) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(CreateHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AttackCreateReservesDef))]
    public sealed class AttackCreateReserves : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd) 
                yield break;
            yield return new AddCardsToHandAction(Library.CreateCards<CreateHaniwa>(Value1));
        }
    }
}
