using LBoL.Base;
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
            cardConfig.Value1 = 3;
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
        protected override int PassiveConsumedRemainingValue => 2;
        protected override int OnPlayConsumedRemainingValue => 3;
        public ManaGroup TotalMana => Mana + ManaGroup.Whites(base.UpgradeCounter.GetValueOrDefault() / 8);
        public override bool IsCavalryType => true;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                yield break;
            if (args.Card.CardType != CardType.Attack)
                yield break;
            if (RemainingValue < PassiveConsumedRemainingValue)
                yield break;
            if (base.Battle.HandZone.Count == base.Battle.MaxHand)
                yield break;

            base.NotifyActivating();
            yield return PerformAction.Wait(0.2f);
            yield return new DrawManyCardAction(Value2);
            RemainingValue -= PassiveConsumedRemainingValue;
            base.NotifyChanged();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainManaAction(TotalMana);
            yield return ConsumeLoyalty();
        }
    }
}
