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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 0;
            cardConfig.Value2 = 3;
            cardConfig.UpgradedValue2 = 5;
            cardConfig.Mana = new ManaGroup() { Any = 1 };
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaSpyDef))]
    public sealed class HaniwaSpy : ModFrontlineCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.TurnEnded, this.OnPlayerTurnEnded);
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
            Card c = cards.Where(c => c.Cost.Any > 0).SampleOrDefault(base.BattleRng);
            if (c == null)
                yield break;
            yield return PerformAction.ViewCard(c);
            c.DecreaseTurnCost(Mana);
            c.NotifyChanged();
            yield return PerformAction.Wait(0.2f);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Card> cards = Battle.DiscardZone.Where(c => c.Cost.Amount == 0).ToList();
            foreach (var card in cards.GetRange(0, Math.Min(cards.Count, Value2)))
            {
                yield return PerformAction.ViewCard(card);
                yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Top);
            };
        }
    }
}
