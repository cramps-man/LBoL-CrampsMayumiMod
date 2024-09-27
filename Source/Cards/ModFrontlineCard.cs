using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public abstract class ModFrontlineCard: Card
    {
        public int RemainingValue { get; set; } = 0;

        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<CardsEventArgs>(base.Battle.CardsAddedToHand, new GameEventHandler<CardsEventArgs>(this.OnCardsAddedToHand));
        }

        private void OnCardsAddedToHand(CardsEventArgs args)
        {
            if (args.Cards.Contains(this))
            {
                RemainingValue = Value1;
                base.NotifyChanged();
            }
        }
        public override IEnumerable<BattleAction> OnDraw()
        {
            RemainingValue = Value1;
            base.NotifyChanged();
            return null;
        }

        public override IEnumerable<BattleAction> OnTurnStartedInHand()
        {
            RemainingValue = Value1;
            base.NotifyChanged();
            return null;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            RemainingValue += Config.UpgradedValue1.GetValueOrDefault() - Config.Value1.GetValueOrDefault();
            base.NotifyChanged();
        }
    }
}
