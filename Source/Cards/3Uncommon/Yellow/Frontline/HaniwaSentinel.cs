using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Intentions;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class HaniwaSentinelDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaSentinel);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.Self;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Block = 4;
            cardConfig.Value1 = 10;
            cardConfig.Value2 = 4;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Weak), nameof(IllnessPreventionSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Weak), nameof(IllnessPreventionSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaSentinelDef))]
    public sealed class HaniwaSentinel : ModFrontlineCard
    {
        public override bool IsArcherType => true;
        protected override int PassiveConsumedRemainingValue => 10;
        protected override int OnPlayConsumedRemainingValue => 5;
        public int PassiveScaling => 10;
        public int PassiveNumTimes => 1 + base.UpgradeCounter.GetValueOrDefault() / 10;
        public int WeakDuration => 1;
        public int IllnessBuffLevelScaling => 5;
        public int IllnessBuffLevel => 1 + base.UpgradeCounter.GetValueOrDefault() / IllnessBuffLevelScaling;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.TurnEnding, this.OnTurnEnding);
        }

        private IEnumerable<BattleAction> OnTurnEnding(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;
            if (CheckPassiveLoyaltyNotFulfiled())
                yield break;
            var attackingEnemies = base.Battle.AllAliveEnemies.Where(e => e.Intentions.Any(i => i is AttackIntention || (i is SpellCardIntention sci && sci.Damage.HasValue)));
            if (!attackingEnemies.Any())
                yield break;

            yield return ConsumePassiveLoyalty();
            for (int i = 0; i < PassiveNumTimes; i++)
            {
                base.NotifyActivating();
                yield return new DamageAction(base.Battle.Player, attackingEnemies, DamageInfo.Reaction(Value2));
                foreach (var debuffAction in DebuffAction<Weak>(attackingEnemies, duration: WeakDuration))
                {
                    yield return debuffAction;
                };
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return BuffAction<IllnessPreventionSe>(level: IllnessBuffLevel);
        }
    }
}
