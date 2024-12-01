using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class HaniwaSpyDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaSpy);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Value1 = 3;
            cardConfig.Scry = 1;
            cardConfig.Mana = new ManaGroup() { Any = 1 };
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeKeyword = Keyword.Scry | Keyword.TempMorph;
            cardConfig.UpgradedRelativeKeyword = Keyword.Scry | Keyword.TempMorph;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaSpyDef))]
    public sealed class HaniwaSpy : ModFrontlineCard
    {
        public ScryInfo TotalScry => Scry.IncreasedBy(Math.Min(base.UpgradeCounter.GetValueOrDefault(), 10));
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.TurnEnded, this.OnPlayerTurnEnded);
            base.ReactBattleEvent(base.Battle.CardMoved, this.OnCardMoved);
        }

        private IEnumerable<BattleAction> OnCardMoved(CardMovingEventArgs args)
        {
            if (base.Zone != CardZone.Hand)
                yield break;
            if (RemainingValue <= 0)
                yield break;
            if (args.SourceZone != CardZone.Draw || args.DestinationZone != CardZone.Discard)
                yield break;
            if (args.Card.Cost.Any <= 0 || args.Card.HasKeyword(Keyword.Forbidden))
                yield break;

            this.NotifyActivating();
            RemainingValue -= 1;
            yield return PerformAction.ViewCard(args.Card);
            args.Card.DecreaseTurnCost(Mana);
            yield return PerformAction.Wait(0.2f);
        }

        private IEnumerable<BattleAction> OnPlayerTurnEnded(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            IReadOnlyList<Card> cards = new List<Card>();
            if (base.Zone == CardZone.Discard)
            {
                cards = base.Battle.DiscardZone;
            }
            else if (base.Zone == CardZone.Draw)
            {
                cards = base.Battle.DrawZone;
            }

            foreach (var item in LowerMana(cards))
            {
                yield return item;
            };
        }

        private IEnumerable<BattleAction> LowerMana(IReadOnlyList<Card> cards)
        {
            Card c = cards.Where(c => c.Cost.Any > 0 && !c.HasKeyword(Keyword.Forbidden)).SampleOrDefault(base.BattleRng);
            if (c == null)
                yield break;
            yield return PerformAction.ViewCard(c);
            c.DecreaseTurnCost(Mana);
            c.NotifyChanged();
            yield return PerformAction.Wait(0.2f);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new ScryAction(TotalScry);
            yield return new DrawCardAction();
        }
    }
}
