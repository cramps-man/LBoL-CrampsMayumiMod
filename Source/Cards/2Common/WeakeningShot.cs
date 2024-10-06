using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class WeakeningShotDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(WeakeningShot);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Damage = 10;
            cardConfig.UpgradedDamage = 13;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 2;
            cardConfig.UpgradedValue2 = 3;
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Weak), nameof(TempFirepowerNegative) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Weak), nameof(TempFirepowerNegative) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(WeakeningShotDef))]
    public sealed class WeakeningShot : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;
 
            int archerCount = HaniwaUtils.GetHaniwaLevel<ArcherHaniwa>(base.Battle.Player);
            if (archerCount >= 1)
                yield return DebuffAction<Weak>(selector.SelectedEnemy, duration: Value1);
            if (archerCount >= 3)
                yield return DebuffAction<TempFirepowerNegative>(selector.SelectedEnemy, level: Value2);
        }
    }
}
