﻿using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class HaniwaSupportDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaSupport);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Value1 = 10;
            cardConfig.Value2 = 1;
            cardConfig.Mana = new ManaGroup() { White = 1 };
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaSupportDef))]
    public sealed class HaniwaSupport : ModFrontlineCard
    {
        protected override int PassiveConsumedRemainingValue => 12;
        protected override int OnPlayConsumedRemainingValue => 10;
        public ManaGroup PMana => ManaGroup.Philosophies((base.UpgradeCounter.GetValueOrDefault() + 5) / 10);
        public ManaGroup TotalMana => PMana + ManaGroup.Whites(1 + (base.UpgradeCounter.GetValueOrDefault() / 10) - PMana.Amount);
        public override bool IsCavalryType => true;
        private static bool SupportDrawnThisCard = false;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.CardUsed, this.OnCardUsed);
            base.HandleBattleEvent(base.Battle.CardUsing, this.OnCardUsing);
        }

        private void OnCardUsing(CardUsingEventArgs args)
        {
            SupportDrawnThisCard = false;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                yield break;
            if (args.Card.Cost != ManaGroup.Empty)
                yield break;
            if (CheckPassiveLoyaltyNotFulfiled())
                yield break;
            if (base.Battle.HandZone.Count == base.Battle.MaxHand)
                yield break;
            if (SupportDrawnThisCard)
                yield break;

            base.NotifyActivating();
            yield return PerformAction.Wait(0.2f);
            yield return new DrawManyCardAction(Value2);
            yield return ConsumePassiveLoyalty();
            SupportDrawnThisCard = true;
            base.NotifyChanged();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainManaAction(TotalMana);
        }
    }
}
