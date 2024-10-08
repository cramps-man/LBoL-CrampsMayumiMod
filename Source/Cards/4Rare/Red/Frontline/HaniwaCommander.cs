using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class HaniwaCommanderDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaCommander);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 3;
            cardConfig.Value2 = 5;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaCommanderDef))]
    public sealed class HaniwaCommander : ModFrontlineCard
    {
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault();
        protected override bool IncludeUpgradesInRemainingValue => true;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.CardUsed, this.OnCardUsed);
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;
            if (!(args.Card is ModFrontlineCard))
                yield break;
            if (RemainingValue <= 0)
                yield break;

            Card card = base.Battle.DrawZone.Where(c => c is ModFrontlineCard).SampleOrDefault(base.BattleRng);
            if (card == null) 
                yield break;
            base.NotifyActivating();
            RemainingValue -= 1;
            yield return new MoveCardAction(card, CardZone.Hand);
        }
        public override Interaction Precondition()
        {
            List<Card> list = new List<Card>();
            if (base.UpgradeCounter >= 5)
                list = base.Battle.HandZone.Where(c => c != this && c is ModFrontlineCard && !(c is HaniwaCommander))
                    .Concat(base.Battle.DrawZone.Where(c => c != this && c is ModFrontlineCard && !(c is HaniwaCommander)))
                    .Concat(base.Battle.DiscardZone.Where(c => c != this && c is ModFrontlineCard && !(c is HaniwaCommander))).ToList();
            else
                list = base.Battle.HandZone.Where(c => c != this && c is ModFrontlineCard && !(c is HaniwaCommander)).ToList();
            return new SelectHandInteraction(0, Value2, list);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction selectInteraction))
                yield break;

            foreach (var card in selectInteraction.SelectedCards)
            {
                card.NotifyActivating();
                foreach (var action in card.GetActions(selector, consumingMana, null, new List<DamageAction>(), false))
                {
                    if (base.Battle.BattleShouldEnd)
                        yield break;
                    yield return action;
                }
            }
        }
    }
}
