using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Utils;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using LBoL.Core.Battle.Interactions;

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
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Keywords = Keyword.Retain | Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaCommanderDef))]
    public sealed class HaniwaCommander : ModFrontlineCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.TurnStarted, this.OnPlayerTurnStarted);
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;

            base.NotifyActivating();
            Type commonType = HaniwaFrontlineUtils.CommonSummonTypes.SampleOrDefault(base.BattleRng);
            yield return new AddCardsToHandAction(Library.CreateCard(commonType));
        }
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where(c => c != this && c is ModFrontlineCard && !(c is HaniwaCommander)).ToList();
            return new SelectHandInteraction(0, Value1, list);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction selectInteraction))
                yield break;

            foreach (var card in selectInteraction.SelectedCards)
            {
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
