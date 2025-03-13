using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
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
            cardConfig.Damage = 15;
            cardConfig.UpgradedDamage = 20;
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Value2 = 2;
            cardConfig.UpgradedValue2 = 3;
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            cardConfig.Keywords = Keyword.Accuracy;
            cardConfig.UpgradedKeywords = Keyword.Accuracy;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Weak), nameof(Vulnerable), nameof(LockedOn), nameof(TempFirepowerNegative) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Weak), nameof(Vulnerable), nameof(LockedOn), nameof(TempFirepowerNegative) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateArcherDef))]
    public sealed class CreateArcher : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainHaniwaAction(archerToGain: Value1);
            yield return base.AttackAction(selector);
            
            int archerCount = HaniwaUtils.GetHaniwaLevel<ArcherHaniwa>(base.Battle.Player);
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (archerCount >= 3)
                yield return DebuffAction<TempFirepowerNegative>(selector.GetEnemy(base.Battle), 3);
            if (archerCount >= 5)
                yield return DebuffAction<LockedOn>(selector.GetEnemy(base.Battle), Value2);
            if (archerCount >= 7)
                yield return DebuffAction<Weak>(selector.GetEnemy(base.Battle), duration: 1);
            if (archerCount >= 10)
                yield return DebuffAction<Vulnerable>(selector.GetEnemy(base.Battle), duration: 1);
        }
    }
}
