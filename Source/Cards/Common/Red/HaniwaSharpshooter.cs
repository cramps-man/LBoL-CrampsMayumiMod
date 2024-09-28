using LBoL.Base;
using LBoL.Base.Extensions;
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
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 1;
            cardConfig.UpgradedValue2 = 2;
            cardConfig.Keywords = Keyword.Retain | Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            cardConfig.RelativeKeyword = Keyword.Accuracy;
            cardConfig.UpgradedRelativeKeyword = Keyword.Accuracy;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Graze), nameof(LockedOn) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Graze), nameof(LockedOn) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaSharpshooterDef))]
    public sealed class HaniwaSharpshooter : ModFrontlineCard
    {
        public int DamageBypassAccurate { get; set; } = 0;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.HandleBattleEvent(base.Battle.Player.DamageDealing, OnPlayerDamageDealing);
            base.HandleBattleEvent(base.Battle.Player.DamageGiving, OnPlayerDamageGiving);
        }

        private void OnPlayerDamageDealing(DamageDealingEventArgs args)
        {
            DamageBypassAccurate = args.DamageInfo.Damage.RoundToInt();
        }

        private void OnPlayerDamageGiving(DamageEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                return;
            if (RemainingValue <= 0)
                return;
            if (!(args.Target is EnemyUnit))
                return;
            if (args.DamageInfo.DamageType != DamageType.Attack)
                return;
            if (args.DamageInfo.IsAccuracy)
                return;
            if (!args.DamageInfo.IsGrazed)
                return;
            if (!args.Target.HasStatusEffect<Graze>())
                return;

            base.NotifyActivating();
            args.AddModifier(this);
            var dmgInfo = args.DamageInfo;
            dmgInfo.Damage = DamageBypassAccurate;
            dmgInfo.IsGrazed = false;
            dmgInfo.IsAccuracy = true;
            args.DamageInfo = args.Target.MeasureDamage(dmgInfo);
            RemainingValue -= 1;
            base.NotifyChanged();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DebuffAction<LockedOn>(selector.GetEnemy(base.Battle), Value2);
        }
    }
}
