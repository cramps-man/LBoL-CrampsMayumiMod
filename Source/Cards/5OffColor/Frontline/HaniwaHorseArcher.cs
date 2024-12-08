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
    public sealed class HaniwaHorseArcherDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaHorseArcher);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Green };
            cardConfig.Damage = 13;
            cardConfig.Value1 = 5;
            cardConfig.Value2 = 6;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish | Keyword.Accuracy;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish | Keyword.Accuracy;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Graze) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Graze) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaHorseArcherDef))]
    public sealed class HaniwaHorseArcher : ModFrontlineCard
    {
        public override bool IsArcherType => true;
        public override bool IsCavalryType => true;
        public int PassiveConsumedFromDrawDiscard => 3;
        protected override int PassiveConsumedRemainingValue => 2;
        protected override int OnPlayConsumedRemainingValue => 6;
        public DamageInfo GrazeDamage => DamageInfo.Attack(Value2);
        public override int AdditionalDamage => base.UpgradeCounter.GetValueOrDefault();
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault();
        public int GrazeGained => 1 + base.UpgradeCounter.GetValueOrDefault() / 10;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.DamageReceived, this.OnPlayerDamageReceived);
        }

        private IEnumerable<BattleAction> OnPlayerDamageReceived(DamageEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            int consumingPassive = 0;
            if (base.Zone == CardZone.Hand)
                consumingPassive = PassiveConsumedRemainingValue;
            else if (base.Zone == CardZone.Draw || base.Zone == CardZone.Discard)
                consumingPassive = PassiveConsumedFromDrawDiscard;
            else
                yield break;
            if (RemainingValue < consumingPassive)
                yield break;
            if (!args.DamageInfo.IsGrazed)
                yield break;

            if (base.Zone == CardZone.Hand)
                base.NotifyActivating();
            else
                yield return PerformAction.ViewCard(this);
            yield return new DamageAction(base.Battle.Player, args.Source, GrazeDamage, "");
            RemainingValue -= consumingPassive;
            base.NotifyChanged();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;
            yield return BuffAction<Graze>(GrazeGained);
            yield return ConsumeLoyalty();
        }
    }
}
