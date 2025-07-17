using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
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
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Value2 = 2;
            cardConfig.UpgradedValue2 = 3;
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            cardConfig.Keywords = Keyword.Accuracy;
            cardConfig.UpgradedKeywords = Keyword.Accuracy;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(LockedOn), nameof(Weak), nameof(Vulnerable) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(LockedOn), nameof(Weak), nameof(Vulnerable) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateArcherDef))]
    public sealed class CreateArcher : ModMayumiCard
    {
        public DamageInfo IncreasedDamage => Damage.IncreaseBy(5);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainHaniwaAction(archerToGain: Value1);
            
            int archerCount = HaniwaUtils.GetHaniwaLevel<ArcherHaniwa>(base.Battle.Player);
            if (archerCount >= 5)
                yield return base.AttackAction(selector, IncreasedDamage);
            else
                yield return base.AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (archerCount >= 3)
                yield return DebuffAction<LockedOn>(selector.GetEnemy(base.Battle), Value2);
            if (archerCount >= 7)
                yield return DebuffAction<Weak>(selector.GetEnemy(base.Battle), duration: 1);
            if (archerCount >= 10)
                yield return DebuffAction<Vulnerable>(selector.GetEnemy(base.Battle), duration: 1);
        }
    }
}
