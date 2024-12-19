using LBoL.Base;
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
            cardConfig.Damage = 25;
            cardConfig.Value1 = 5;
            cardConfig.Value2 = 2;
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
        public override int AdditionalDamage => base.UpgradeCounter.GetValueOrDefault() * 2;
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
            if (base.Zone != CardZone.Draw && base.Zone != CardZone.Hand && base.Zone != CardZone.Discard)
                yield break;
            if (RemainingValue < PassiveConsumedRemainingValue)
                yield break;
            var frontlinesInHand = base.Battle.HandZone.Where(c => c != this && c is ModFrontlineCard && !(c is FrontlineCommander)).ToList();
            if (!frontlinesInHand.Any())
                yield break;

            if (base.Zone == CardZone.Hand)
            {
                base.NotifyActivating();
                yield return PerformAction.Wait(0.3f);
            }
            else if (base.Zone == CardZone.Draw || base.Zone == CardZone.Discard)
                yield return PerformAction.ViewCard(this);

            foreach (var battleAction in HaniwaFrontlineUtils.ExecuteOnPlayActions(frontlinesInHand, base.Battle))
            {
                yield return battleAction;
            }
            RemainingValue -= PassiveConsumedRemainingValue;
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
