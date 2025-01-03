using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class KeikiTeammateDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(KeikiTeammate);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Friend;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White, ManaColor.Blue };
            cardConfig.Cost = new ManaGroup() { Red = 1, White = 1, Blue = 1 };
            cardConfig.Value1 = 8;
            cardConfig.UpgradedValue1 = 10;
            cardConfig.Value2 = 8;
            cardConfig.UpgradedValue2 = 12;
            cardConfig.Loyalty = 2;
            cardConfig.UpgradedLoyalty = 2;
            cardConfig.PassiveCost = 1;
            cardConfig.UpgradedPassiveCost = 2;
            cardConfig.ActiveCost = -4;
            cardConfig.UpgradedActiveCost = -4;
            cardConfig.UltimateCost = -8;
            cardConfig.UpgradedUltimateCost = -8;
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaUpgrader), nameof(HaniwaExploiter) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaUpgrader), nameof(HaniwaExploiter) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(KeikiTeammateDef))]
    public sealed class KeikiTeammate : Card
    {
        public int FrontlineExileDamage
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                return ExileCards.Count() * Value2;
            }
        }
        public IEnumerable<Card> ExileCards => base.Battle.ExileZone.Where(c => c is ModFrontlineCard);
        public override IEnumerable<BattleAction> OnTurnStartedInHand()
        {
            return this.GetPassiveActions();
        }
        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!base.Summoned || base.Battle.BattleShouldEnd)
                yield break;
            base.NotifyActivating();
            base.Loyalty += base.PassiveCost;
            IEnumerable<Type> possibleSummons = HaniwaFrontlineUtils.GetAllSummonTypes(base.Battle);
            List<Card> cards = new List<Card>();
            for (int i = 0; i < 2; i++)
                cards.Add(Library.CreateCard(possibleSummons.Sample(base.BattleRng)));
            yield return new AddCardsToHandAction(cards);
        }
        public override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            return base.SummonActions(selector, consumingMana, precondition);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
            {
                base.Loyalty += base.ActiveCost;
                List<StatusEffect> removedAssignBuffs = base.Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect).ToList();
                foreach (ModAssignStatusEffect statusEffect in removedAssignBuffs)
                {
                    foreach (var item in statusEffect.ImmidiatelyTrigger())
                    {
                        yield return item;
                    }
                }
                yield return DefenseAction(0, Value1 * removedAssignBuffs.Count);
                yield return base.SkillAnime;
            }
            else
            {
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                yield return new DamageAction(base.Battle.Player, base.Battle.AllAliveEnemies, DamageInfo.Attack(FrontlineExileDamage));
                foreach (var card in ExileCards.ToList())
                {
                    yield return new MoveCardAction(card, CardZone.Discard);
                }
                yield return base.SkillAnime;
            }
        }
    }
}
