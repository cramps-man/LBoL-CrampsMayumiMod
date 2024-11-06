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
    public sealed class HaniwaCommanderDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaCommander);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Damage = 25;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(CommandersMarkSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(CommandersMarkSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaCommanderDef))]
    public sealed class HaniwaCommander : ModFrontlineCard
    {
        public override int AdditionalDamage => base.UpgradeCounter.GetValueOrDefault() * 2;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.TurnStarted, this.OnTurnStarted);
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Draw && base.Zone != CardZone.Hand && base.Zone != CardZone.Discard)
                yield break;
            if (RemainingValue <= 0)
                yield break;
            var frontlinesInHand = base.Battle.HandZone.Where(c => c != this && c is ModFrontlineCard && !(c is HaniwaCommander)).ToList();
            if (!frontlinesInHand.Any())
                yield break;

            if (base.Zone == CardZone.Hand)
            {
                base.NotifyActivating();
                yield return PerformAction.Wait(0.3f);
            }
            else if (base.Zone == CardZone.Draw || base.Zone == CardZone.Discard)
                yield return PerformAction.ViewCard(this);

            foreach (var card in frontlinesInHand)
            {
                card.NotifyActivating();
                foreach (var action in card.GetActions(HaniwaFrontlineUtils.GetTargetForOnPlayAction(base.Battle), new ManaGroup(), null, new List<DamageAction>(), false))
                {
                    if (base.Battle.BattleShouldEnd)
                        yield break;
                    yield return action;
                }
            }
            RemainingValue -= 1;
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
