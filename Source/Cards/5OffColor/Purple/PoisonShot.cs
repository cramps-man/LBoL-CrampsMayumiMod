using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class PoisonShotDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PoisonShot);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Black };
            cardConfig.Damage = 12;
            cardConfig.Cost = new ManaGroup() { Black = 1, Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Poison) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Poison) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(PoisonShotDef))]
    public sealed class PoisonShot : Card
    {
        public int Level0Poison => IsUpgraded ? 5 : 3;
        public int Level1Poison => IsUpgraded ? 10 : 6;
        public int Level2Poison => IsUpgraded ? 15 : 10;
        public int Level3Poison => IsUpgraded ? 20 : 15;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;
 
            int archerCount = HaniwaUtils.GetHaniwaLevel<ArcherHaniwa>(base.Battle.Player);
            int poisonValue = 0;
            if (archerCount >= 1)
                poisonValue = Level0Poison;
            if (archerCount >= 3)
                poisonValue = Level1Poison;
            if (archerCount >= 5)
                poisonValue = Level2Poison;
            if (archerCount >= 10)
                poisonValue = Level3Poison;
            if (poisonValue <= 0)
                yield break;

            if (archerCount >= 7)
            {
                foreach (var item in DebuffAction<Poison>(base.Battle.AllAliveEnemies, level: poisonValue))
                {
                    yield return item;
                };
            }
            else
            {
                yield return DebuffAction<Poison>(selector.GetEnemy(base.Battle), level: poisonValue);
            }
        }
    }
}
