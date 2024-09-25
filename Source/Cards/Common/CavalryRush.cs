using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CavalryRushDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CavalryRush);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1, Any = 1 };
            cardConfig.Damage = 18;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 2;
            cardConfig.Mana = new ManaGroup() { Red = 1 };
            cardConfig.UpgradedMana = new ManaGroup { Red = 2 };
            cardConfig.UpgradedKeywords = Keyword.Accuracy;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalryRushDef))]
    public sealed class CavalryRush : Card
    {
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled<CavalryHaniwa>(base.Battle.Player, Value2, HaniwaActionType.Sacrifice);
        public override string CantUseMessage => "Need more Cavalry";

        protected override void OnEnterBattle(BattleController battle)
        {
            ReactBattleEvent(base.Battle.EnemyDied, this.OnEnemyDied);
        }

        private IEnumerable<BattleAction> OnEnemyDied(DieEventArgs args)
        {
            if (args.DieSource == this)
            {
                yield return new GainManaAction(Mana);
                yield return new DrawManyCardAction(Value1);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, cavalryToLose: Value2);
            yield return AttackAction(selector);
        }
    }
}
