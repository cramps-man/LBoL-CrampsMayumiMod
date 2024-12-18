using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Damage = 10;
            cardConfig.Value1 = 5;
            cardConfig.Value2 = 6;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaAttackerDef))]
    public sealed class HaniwaAttacker : ModFrontlineCard
    {
        protected override int PassiveConsumedRemainingValue => 2;
        protected override int OnPlayConsumedRemainingValue => 3;
        public DamageInfo EndOfTurnDmg => DamageInfo.Reaction(Value2);
        public override int AdditionalDamage => base.UpgradeCounter.GetValueOrDefault();
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault();
        public override bool IsFencerType => true;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.TurnEnding, this.OnPlayerTurnEnding);
        }

        private IEnumerable<BattleAction> OnPlayerTurnEnding(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;
            if (RemainingValue < PassiveConsumedRemainingValue)
                yield break;

            base.NotifyActivating();
            yield return PerformAction.Wait(0.2f);
            yield return new DamageAction(base.Battle.Player, base.Battle.LowestHpEnemy, EndOfTurnDmg);
            RemainingValue -= PassiveConsumedRemainingValue;
            base.NotifyChanged();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            yield return ConsumeLoyalty();
        }
    }
}
