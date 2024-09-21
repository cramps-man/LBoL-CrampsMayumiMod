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
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class SupportFrontDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(SupportFront);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.Self;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 1;
            cardConfig.Mana = new ManaGroup() { Red = 1 };
            cardConfig.Keywords = Keyword.Retain | Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Sacrifice) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(SupportFrontDef))]
    public sealed class SupportFront : Card
    {
        public int RemainingDraw { get; set; } = 0;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.HandleBattleEvent<CardsEventArgs>(base.Battle.CardsAddedToHand, new GameEventHandler<CardsEventArgs>(this.OnCardsAddedToHand));
        }

        private void OnCardsAddedToHand(CardsEventArgs args)
        {
            if (args.Cards.Contains(this))
            {
                RemainingDraw = Value1;
                base.NotifyChanged();
            }
        }

        public override IEnumerable<BattleAction> OnDraw()
        {
            RemainingDraw = Value1;
            base.NotifyChanged();
            return null;
        }

        public override IEnumerable<BattleAction> OnTurnStartedInHand()
        {
            RemainingDraw = Value1;
            base.NotifyChanged();
            return null;
        }
        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                yield break;
            if (args.Card.CardType != CardType.Attack)
                yield break;
            if (RemainingDraw <= 0)
                yield break;

            base.NotifyActivating();
            yield return PerformAction.Wait(0.2f);
            yield return new DrawManyCardAction(Value2);
            RemainingDraw -= 1;
            base.NotifyChanged();
        }
        public override void Upgrade()
        {
            base.Upgrade();
            RemainingDraw += Config.UpgradedValue1.GetValueOrDefault() - Config.Value1.GetValueOrDefault();
            base.NotifyChanged();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainManaAction(Mana);
        }
    }
}
