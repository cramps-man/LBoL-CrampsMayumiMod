using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

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
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Cost = new ManaGroup() { Black = 1, Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Poison) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Poison) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(PoisonShotDef))]
    public sealed class PoisonShot : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;

            int poisonValue = Value1 + Value1 * selector.GetEnemy(base.Battle).StatusEffects.Where(s => s.Type == StatusEffectType.Negative).Count();
            yield return DebuffAction<Poison>(selector.GetEnemy(base.Battle), level: poisonValue);
        }
    }
}
