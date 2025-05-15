using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class HaniwaBodyguardDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaBodyguard);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Block = 8;
            cardConfig.Value1 = 5;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.Illustrator = "camellia";
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaBodyguardDef))]
    public sealed class HaniwaBodyguard : ModFrontlineCard
    {
        public override int AdditionalBlock => base.UpgradeCounter.GetValueOrDefault();
        protected override int OnPlayConsumedRemainingValue => 5;
        public int DamageTaken { get; set; } = 0;
        public override bool IsFencerType => true;
        public int TurnStartLoyaltyScaling => 2;
        public int LoyaltyThreshold => 3 + base.UpgradeCounter.GetValueOrDefault() / TurnStartLoyaltyScaling;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.DamageTaking, this.OnPlayerDamageTaking);
            base.HandleBattleEvent(base.Battle.Player.TurnStarting, this.OnPlayerTurnStarting);
        }

        private void OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (base.Battle.HandZone.Contains(this) && RemainingValue < LoyaltyThreshold)
                RemainingValue = LoyaltyThreshold;
        }

        private IEnumerable<BattleAction> OnPlayerDamageTaking(DamageEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                yield break;
            if (RemainingValue <= 0)
                yield break;
            
            var damageInfo = args.DamageInfo;
            if (damageInfo.DamageType == DamageType.Attack || damageInfo.DamageType == DamageType.Reaction)
            {
                DamageTaken = 0;
                if (damageInfo.Damage > 0)
                {
                    int reduceDamageBy = Math.Min(damageInfo.Damage.RoundToInt(), RemainingValue);
                    damageInfo = damageInfo.ReduceActualDamageBy(reduceDamageBy);
                    yield return new ConsumeLoyaltyAction(this, reduceDamageBy);
                    DamageTaken += reduceDamageBy;
                }
                if (damageInfo.DamageShielded > 0)
                {
                    int reduction = Math.Min(damageInfo.DamageShielded.RoundToInt(), RemainingValue);
                    damageInfo.DamageShielded -= reduction;
                    yield return new ConsumeLoyaltyAction(this, reduction);
                    DamageTaken += reduction;
                }
                if (damageInfo.DamageBlocked > 0) 
                {
                    /*int reduction = Math.Min(damageInfo.DamageBlocked.RoundToInt(), RemainingValue);
                    damageInfo.DamageBlocked -= reduction;
                    RemainingValue -= reduction;
                    DamageTaken += reduction;*/
                }
                if (DamageTaken != 0)
                {
                    base.NotifyActivating();
                    int powerToLose = DamageTaken;
                    DamageTaken = 0;
                    args.AddModifier(this);
                    args.DamageInfo = damageInfo;
                    base.NotifyChanged();
                    if (IsDarknessMode)
                    {
                        if (powerToLose > base.Battle.Player.Power)
                            yield return new RemoveCardAction(this);
                        yield return new LosePowerAction(powerToLose);
                    }
                }
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
        }
    }
}
