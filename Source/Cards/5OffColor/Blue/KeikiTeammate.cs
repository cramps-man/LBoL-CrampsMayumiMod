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
            cardConfig.Value2 = 10;
            cardConfig.UpgradedValue2 = 16;
            cardConfig.Loyalty = 2;
            cardConfig.UpgradedLoyalty = 2;
            cardConfig.PassiveCost = 1;
            cardConfig.UpgradedPassiveCost = 2;
            cardConfig.ActiveCost = -4;
            cardConfig.UpgradedActiveCost = -4;
            cardConfig.UltimateCost = -9;
            cardConfig.UpgradedUltimateCost = -9;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
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
        public IEnumerable<Card> ExileCards => base.Battle.HandZone.Where(c => c != this && c is ModFrontlineCard)
                    .Concat(base.Battle.DrawZone.Where(c => c != this && c is ModFrontlineCard))
                    .Concat(base.Battle.DiscardZone.Where(c => c != this && c is ModFrontlineCard));
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
            IEnumerable<Type> possibleSummons = HaniwaFrontlineUtils.CommonSummonTypes.Concat(HaniwaFrontlineUtils.UncommonSummonTypes);
            yield return new AddCardsToHandAction(Library.CreateCard(possibleSummons.Sample(base.BattleRng)));
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
                    statusEffect.TickdownFull();
                }
                yield return DefenseAction(0, Value1 * removedAssignBuffs.Count);
                yield return base.SkillAnime;
            }
            else
            {
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                yield return new DamageAction(base.Battle.Player, base.Battle.AllAliveEnemies, DamageInfo.Attack(FrontlineExileDamage));
                yield return new ExileManyCardAction(ExileCards);
                yield return base.SkillAnime;
            }
        }
    }
}
