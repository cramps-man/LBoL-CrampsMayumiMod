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
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Block = 10;
            cardConfig.Value1 = 4;
            cardConfig.Value2 = 1;
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
        protected override int OnPlayConsumedRemainingValue => 5;
        public int NumCardsToUpgrade => 2;
        public int NumUpgradedCards
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                return base.Battle.HandZoneAndPlayArea.Where(c => c.IsUpgraded).Count();
            }
        }
        public int NumUpgradedCardsBlock => NumUpgradedCards * Value2;
        public override int AdditionalBlock => base.UpgradeCounter.GetValueOrDefault() + NumUpgradedCardsBlock;
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault() / 5;
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

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return UpgradeAllHandsAction();
        }
    }
}
