using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class HaniwaAssassinDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaAssassin);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            cardConfig.Damage = 13;
            cardConfig.Value1 = 15;
            cardConfig.Value2 = 20;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeCards = new List<string>() { nameof(Knife) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(Knife) };
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaAssassinDef))]
    public sealed class HaniwaAssassin : ModFrontlineCard
    {
        public override bool IsFencerType => true;
        public int PassiveConsumedFromDrawDiscard => 3;
        protected override int PassiveConsumedRemainingValue => 3;
        protected override int OnPlayConsumedRemainingValue => 5;
        public override int AdditionalDamage => base.UpgradeCounter.GetValueOrDefault();
        public int FollowupDamage => 6 + base.UpgradeCounter.GetValueOrDefault();
        public int InstakillScaling => 2;
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault() * InstakillScaling;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.DamageDealt, this.OnPlayerDamageDealt);
            base.ReactBattleEvent(base.Battle.Player.TurnEnding, this.OnPlayerTurnEnding);
        }

        private IEnumerable<BattleAction> OnPlayerTurnEnding(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Draw && base.Zone != CardZone.Discard)
                yield break;
            if (CheckPassiveLoyaltyNotFulfiled(PassiveConsumedFromDrawDiscard))
                yield break;
            if (base.Battle.HandZone.Count == base.Battle.MaxHand)
                yield break;

            yield return PerformAction.ViewCard(this);
            yield return new AddCardsToHandAction(Library.CreateCard<Knife>());
            yield return ConsumePassiveLoyalty(PassiveConsumedFromDrawDiscard);
            base.NotifyChanged();
        }

        private IEnumerable<BattleAction> OnPlayerDamageDealt(DamageEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;
            if (!(args.Target is EnemyUnit))
                yield break;
            if (args.ActionSource == this)
                yield break;
            if (args.Target.IsDead)
                yield break;
            if (CheckPassiveLoyaltyNotFulfiled())
                yield break;

            yield return PerformAction.Wait(0.3f);
            base.NotifyActivating();
            yield return DamageAction.Reaction(args.Target, FollowupDamage);
            yield return ConsumePassiveLoyalty();
            base.NotifyChanged();
        }


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            EnemyUnit selectedEnemy = selector.SelectedEnemy;
            if (selectedEnemy.Hp <= base.Value2)
                yield return new ForceKillAction(base.Battle.Player, selectedEnemy);
            else
                yield return AttackAction(selector);
        }
    }
}
