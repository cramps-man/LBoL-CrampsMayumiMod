using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
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
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Damage = 10;
            cardConfig.UpgradedDamage = 15;
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
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
                yield return new GainHaniwaAction(archerToGain: Value1);
            }
        }
    }
}
