using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class HaniwaAttackerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaAttacker);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 8;
            cardConfig.UpgradedDamage = 10;
            cardConfig.Value1 = 6;
            cardConfig.UpgradedValue1 = 8;
            cardConfig.Value2 = 1;
            cardConfig.Keywords = Keyword.Retain | Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Vulnerable) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Vulnerable) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaAttackerDef))]
    public sealed class HaniwaAttacker : ModFrontlineCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.TurnEnded, this.OnPlayerTurnEnded);
        }

        private IEnumerable<BattleAction> OnPlayerTurnEnded(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;
            if (RemainingValue <= 0)
                yield break;

            base.NotifyActivating();
            yield return PerformAction.Wait(0.2f);
            yield return new DamageAction(base.Battle.Player, base.Battle.LowestHpEnemy, DamageInfo.Attack(Value1));
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (!base.Battle.BattleShouldEnd)
                yield return DebuffAction<Vulnerable>(selector.GetEnemy(base.Battle), duration: Value2);
        }
    }
}
