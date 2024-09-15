using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CreateArcherDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CreateArcher);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Green };
            cardConfig.Damage = 7;
            cardConfig.UpgradedDamage = 10;
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 9 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(ArcherHaniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(ArcherHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateArcherDef))]
    public sealed class CreateArcher : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector);
            if (!base.Battle.BattleShouldEnd)
            {
                foreach (var item in HaniwaUtils.ApplyStance<ArcherHaniwa>(base.Battle.Player, Value1))
                {
                    yield return item;
                }
            }
        }
    }
}
