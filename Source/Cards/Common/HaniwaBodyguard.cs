using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
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
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.Nobody;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Value1 = 5;
            cardConfig.UpgradedValue1 = 8;
            cardConfig.Keywords = Keyword.Retain | Keyword.Forbidden;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Forbidden;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaBodyguardDef))]
    public sealed class HaniwaBodyguard : Card
    {
        public int RemainingDamage { get; set; } = 0;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<DamageEventArgs>(base.Battle.Player.DamageTaking, new GameEventHandler<DamageEventArgs>(this.OnPlayerDamageTaking));
            base.ReactBattleEvent<DamageEventArgs>(base.Battle.Player.DamageReceived, new EventSequencedReactor<DamageEventArgs>(this.OnPlayerDamageReceived));
        }

        public override IEnumerable<BattleAction> OnDraw()
        {
            RemainingDamage = Value1;
            return null;
        }

        private IEnumerable<BattleAction> OnPlayerDamageReceived(DamageEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                yield break;
            if (RemainingDamage == 0)
            {
                yield return PerformAction.Wait(0.3f);
                yield return new DiscardAction(this);
            }
        }

        private void OnPlayerDamageTaking(DamageEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                return;

            var damageInfo = args.DamageInfo;
            int damageTaken = damageInfo.Damage.RoundToInt();
            if (damageTaken >= 1 && damageInfo.DamageType == DamageType.Attack)
            {
                base.NotifyActivating();
                int reduceDamageBy = Math.Min(damageTaken, RemainingDamage);
                args.DamageInfo = damageInfo.ReduceActualDamageBy(reduceDamageBy);
                args.AddModifier(this);
                if (RemainingDamage > damageTaken)
                {
                    RemainingDamage -= damageTaken;
                }
                else
                {
                    RemainingDamage = 0;
                }
                base.NotifyChanged();
            }
        }
    }
}
