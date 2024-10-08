using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;

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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Block = 8;
            cardConfig.Value1 = 4;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaBodyguardDef))]
    public sealed class HaniwaBodyguard : ModFrontlineCard
    {
        public override int AdditionalBlock => base.UpgradeCounter.GetValueOrDefault() * 2;
        protected override bool IncludeUpgradesInRemainingValue => true;
        public int DamageTaken { get; set; } = 0;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.HandleBattleEvent<DamageEventArgs>(base.Battle.Player.DamageTaking, new GameEventHandler<DamageEventArgs>(this.OnPlayerDamageTaking));
        }

        private void OnPlayerDamageTaking(DamageEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                return;
            if (RemainingValue <= 0)
                return;
            
            var damageInfo = args.DamageInfo;
            if (damageInfo.DamageType == DamageType.Attack || damageInfo.DamageType == DamageType.Reaction)
            {
                DamageTaken = 0;
                if (damageInfo.Damage > 0)
                {
                    int reduceDamageBy = Math.Min(damageInfo.Damage.RoundToInt(), RemainingValue);
                    damageInfo = damageInfo.ReduceActualDamageBy(reduceDamageBy);
                    RemainingValue -= reduceDamageBy;
                    DamageTaken += reduceDamageBy;
                }
                if (damageInfo.DamageShielded > 0)
                {
                    int reduction = Math.Min(damageInfo.DamageShielded.RoundToInt(), RemainingValue);
                    damageInfo.DamageShielded -= reduction;
                    RemainingValue -= reduction;
                    DamageTaken += reduction;
                }
                if (damageInfo.DamageBlocked > 0) 
                {
                    int reduction = Math.Min(damageInfo.DamageBlocked.RoundToInt(), RemainingValue);
                    damageInfo.DamageBlocked -= reduction;
                    RemainingValue -= reduction;
                    DamageTaken += reduction;
                }
                if (DamageTaken != 0)
                {
                    base.NotifyActivating();
                    DamageTaken = 0;
                    args.AddModifier(this);
                    args.DamageInfo = damageInfo;
                    base.NotifyChanged();
                }
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
        }
    }
}
