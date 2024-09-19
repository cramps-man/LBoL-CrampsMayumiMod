using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

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
            cardConfig.TargetType = TargetType.Nobody;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Value1 = 2;
            cardConfig.Value2 = 1;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Keywords = Keyword.Retain | Keyword.Forbidden;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Forbidden;
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
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                yield break;
            if (args.Card.CardType != CardType.Attack)
                yield break;

            base.NotifyActivating();
            yield return new DrawCardAction();
            RemainingDraw -= 1;
            base.NotifyChanged();
            if (RemainingDraw <= 0)
            {
                yield return PerformAction.Wait(0.3f);
                yield return new DiscardAction(this);
            }
        }

        public override IEnumerable<BattleAction> OnDraw()
        {
            RemainingDraw = Value1;
            return null;
        }
    }
}
