using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class GuardRushDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(GuardRush);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1, Any = 1 };
            cardConfig.Damage = 14;
            cardConfig.UpgradedDamage = 18;
            cardConfig.Block = 10;
            cardConfig.UpgradedBlock = 13;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice), nameof(Graze) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice), nameof(Graze) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(GuardRushDef))]
    public sealed class GuardRush : Card
    {
        public override bool CanUse
        {
            get
            {
                var player = base.Battle.Player;
                return HaniwaUtils.IsLevelFulfilled(player, HaniwaActionType.Sacrifice, Value2, 0, Value2);
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, Value2, 0, Value2);
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;
            yield return DefenseAction();
            yield return BuffAction<Graze>(Value1);
        }
    }
}
