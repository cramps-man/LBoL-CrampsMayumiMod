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
    public sealed class HaniwaUpgraderDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaUpgrader);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Block = 5;
            cardConfig.Value1 = 5;
            cardConfig.Value2 = 2;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaUpgraderDef))]
    public sealed class HaniwaUpgrader : ModFrontlineCard
    {
        public override bool IsFencerType => true;
        protected override int PassiveConsumedRemainingValue => 2;
        protected override int OnPlayConsumedRemainingValue => 4;
        public int NumCardsScaling => 3;
        public int NumCardsToUpgrade => 2 + base.UpgradeCounter.GetValueOrDefault() / NumCardsScaling;
        public int OnPlayTimesToUpgradeScaling => 8;
        public int OnPlayTimesToUpgrade => 2 + base.UpgradeCounter.GetValueOrDefault() / OnPlayTimesToUpgradeScaling;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.Player.TurnEnding, this.OnTurnEnding);
        }

        private IEnumerable<BattleAction> OnTurnEnding(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;
            if (CheckPassiveLoyaltyNotFulfiled())
                yield break;

            IEnumerable<Card> toUpgrade = base.Battle.HandZone.Where(c => c.CanUpgradeAndPositive && c != this).SampleManyOrAll(NumCardsToUpgrade, base.BattleRng);
            if (toUpgrade.Count() < NumCardsToUpgrade)
                toUpgrade = toUpgrade.Append(this);
            this.NotifyActivating();
            yield return ConsumePassiveLoyalty();
            yield return new UpgradeCardsAction(toUpgrade);
        }

        public override Interaction Precondition()
        {
            IEnumerable<Card> cards = base.Battle.HandZone.Where(c => c.CanUpgradeAndPositive);
            var interaction = new SelectCardInteraction(0, Value2, cards);
            interaction.Description = ExtraDescription1.RuntimeFormat(FormatWrapper);
            return interaction;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            if (!(precondition is SelectCardInteraction selectInteraction))
                yield break;
            if (selectInteraction.SelectedCards == null)
                yield break;
            if (!selectInteraction.SelectedCards.Any())
                yield break;
            for (int i = 0; i < OnPlayTimesToUpgrade; i++)
                if (selectInteraction.SelectedCards.Any(c => c.CanUpgradeAndPositive))
                    yield return new UpgradeCardsAction(selectInteraction.SelectedCards.Where(c => c.CanUpgradeAndPositive));
        }
    }
}
