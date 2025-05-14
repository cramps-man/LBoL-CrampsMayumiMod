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
using System.Linq;

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
            cardConfig.Value1 = 10;
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
        protected override int PassiveConsumedRemainingValue => 7;
        protected override int OnPlayConsumedRemainingValue => 3;
        public override int AdditionalDamage => base.UpgradeCounter.GetValueOrDefault();
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault() / 3;
        private bool accuracyModified = false;
        public override bool IsArcherType => true;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.HandleBattleEvent(base.Battle.Player.DamageDealing, OnPlayerDamageDealing);
            base.ReactBattleEvent(base.Battle.Player.DamageGiving, OnPlayerDamageGiving);
        }

        private void OnPlayerDamageDealing(DamageDealingEventArgs args)
        {
            if (args.Cause == ActionCause.OnlyCalculate)
                return;
            accuracyModified = false;
            if (base.Zone != CardZone.Hand)
                return;
            if (CheckPassiveLoyaltyNotFulfiled())
                return;
            if (args.DamageInfo.DamageType != DamageType.Attack)
                return;
            if (args.DamageInfo.IsAccuracy)
                return;
            if (!args.Targets.Where(t => t is EnemyUnit).Any())
                return;
            if (!args.Targets.Where(t => t.HasStatusEffect<Graze>()).Any())
                return;

            var dmgInfo = args.DamageInfo;
            dmgInfo.IsAccuracy = true;
            args.DamageInfo = dmgInfo;
            args.AddModifier(this);

            accuracyModified = true;
        }

        private IEnumerable<BattleAction> OnPlayerDamageGiving(DamageEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                yield break;
            if (CheckPassiveLoyaltyNotFulfiled())
                yield break;
            if (!(args.Target is EnemyUnit))
                yield break;
            if (args.DamageInfo.DamageType != DamageType.Attack)
                yield break;
            if (!args.Target.HasStatusEffect<Graze>())
                yield break;
            if (!accuracyModified)
                yield break;

            base.NotifyActivating();
            yield return ConsumePassiveLoyalty();
            accuracyModified = false;
            base.NotifyChanged();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (!base.Battle.BattleShouldEnd)
                yield return DebuffAction<LockedOn>(selector.GetEnemy(base.Battle), Value2);
        }
    }
}
