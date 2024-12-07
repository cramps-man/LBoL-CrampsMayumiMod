using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class HaniwaSharpshooterDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaSharpshooter);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Damage = 5;
            cardConfig.Value1 = 3;
            cardConfig.Value2 = 1;
            cardConfig.Keywords = Keyword.Accuracy | Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Accuracy | Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Graze), nameof(LockedOn) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Graze), nameof(LockedOn) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaSharpshooterDef))]
    public sealed class HaniwaSharpshooter : ModFrontlineCard
    {
        protected override int PassiveConsumedRemainingValue => 2;
        public override int AdditionalDamage => base.UpgradeCounter.GetValueOrDefault();
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault() / 3;
        private bool accuracyModified = false;
        public override bool IsArcherType => true;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.HandleBattleEvent(base.Battle.Player.DamageDealing, OnPlayerDamageDealing);
            base.HandleBattleEvent(base.Battle.Player.DamageGiving, OnPlayerDamageGiving);
        }

        private void OnPlayerDamageDealing(DamageDealingEventArgs args)
        {
            if (args.Cause == ActionCause.OnlyCalculate)
                return;
            if (base.Zone != CardZone.Hand)
                return;
            if (RemainingValue < PassiveConsumedRemainingValue)
                return;
            if (args.DamageInfo.DamageType != DamageType.Attack)
                return;
            if (args.DamageInfo.IsAccuracy)
                return;

            var dmgInfo = args.DamageInfo;
            dmgInfo.IsAccuracy = true;
            args.DamageInfo = dmgInfo;
            args.AddModifier(this);

            accuracyModified = true;
        }

        private void OnPlayerDamageGiving(DamageEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                return;
            if (RemainingValue < PassiveConsumedRemainingValue)
                return;
            if (!(args.Target is EnemyUnit))
                return;
            if (args.DamageInfo.DamageType != DamageType.Attack)
                return;
            if (!args.Target.HasStatusEffect<Graze>())
                return;
            if (!accuracyModified)
                return;

            base.NotifyActivating();
            RemainingValue -= PassiveConsumedRemainingValue;
            accuracyModified = false;
            base.NotifyChanged();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (!base.Battle.BattleShouldEnd)
                yield return DebuffAction<LockedOn>(selector.GetEnemy(base.Battle), Value2);
            yield return ConsumeLoyalty();
        }
    }
}
