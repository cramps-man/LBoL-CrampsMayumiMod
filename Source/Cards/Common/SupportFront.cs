using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
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
            cardConfig.TargetType = TargetType.Self;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Value2 = 1;
            cardConfig.Mana = new ManaGroup() { Red = 1 };
            cardConfig.Keywords = Keyword.Retain | Keyword.Forbidden;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Sacrifice) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(SupportFrontDef))]
    public sealed class SupportFront : Card
    {
        public int CavalrySacrifice => 2;
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
            if (RemainingDraw > 0)
                yield break;
            if (HaniwaUtils.IsLevelFulfilled<CavalryHaniwa>(base.Battle.Player, CavalrySacrifice))
            {
                yield return PerformAction.Wait(0.3f);
                yield return HaniwaUtils.SacrificeHaniwa<CavalryHaniwa>(base.Battle.Player, CavalrySacrifice);
                yield return new DiscardAction(this);
            }
            else
            {
                yield return new ExileCardAction(this);
            }
        }

        public override IEnumerable<BattleAction> OnDraw()
        {
            RemainingDraw = Value1;
            return null;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainManaAction(Mana);
        }
    }
}
