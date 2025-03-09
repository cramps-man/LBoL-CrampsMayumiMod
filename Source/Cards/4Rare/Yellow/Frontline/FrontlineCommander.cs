using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class FrontlineCommanderDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineCommander);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Damage = 20;
            cardConfig.Value1 = 10;
            cardConfig.Value2 = 3;
            cardConfig.Keywords = Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(CommandersMarkSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(CommandersMarkSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrontlineCommanderDef))]
    public sealed class FrontlineCommander : ModFrontlineCard
    {
        protected override int PassiveConsumedRemainingValue => 5;
        protected override int OnPlayConsumedRemainingValue => 0;
        public override int AdditionalDamage => base.UpgradeCounter.GetValueOrDefault() * 2;
        public int PassiveCommandCount => 3 + base.UpgradeCounter.GetValueOrDefault() / PassiveScaling;
        public int PassiveScaling => 3;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.TurnStarted, this.OnTurnStarted);
            base.HandleBattleEvent(base.Battle.Player.TurnEnded, this.OnTurnEnded);
        }

        private void OnTurnEnded(UnitEventArgs args)
        {
            RemainingValue += Value2;
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;
            if (CheckPassiveLoyaltyNotFulfiled())
                yield break;
            var frontlinesInHand = base.Battle.HandZone.Where(c => c != this && c is ModFrontlineCard).ToList();
            if (!frontlinesInHand.Any())
                yield break;

            base.NotifyActivating();
            yield return PerformAction.Wait(0.3f);
            foreach (var battleAction in HaniwaFrontlineUtils.ExecuteOnPlayActions(frontlinesInHand.SampleManyOrAll(PassiveCommandCount, base.BattleRng).ToList(), base.Battle))
            {
                yield return battleAction;
            }
            yield return ConsumePassiveLoyalty();
            base.NotifyChanged();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;

            yield return DebuffAction<CommandersMarkSe>(selector.GetEnemy(base.Battle));
        }
    }
}
