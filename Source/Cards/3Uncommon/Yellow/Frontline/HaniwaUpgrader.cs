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
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 2;
            cardConfig.Keywords = Keyword.Exile | Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Exile | Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaUpgraderDef))]
    public sealed class HaniwaUpgrader : ModFrontlineCard
    {
        private bool JustSummoned { get; set; } = true;
        public override int AdditionalBlock => Value2 * base.UpgradeCounter.GetValueOrDefault();
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
            if (JustSummoned)
            {
                JustSummoned = false;
                yield break;
            }
            if (RemainingValue <= 0)
                yield break;

            Card toUpgrade = base.Battle.HandZone.Where(c => c.CanUpgradeAndPositive && !(c is HaniwaUpgrader)).SampleOrDefault(base.BattleRng);
            if (toUpgrade == null)
                yield break;
            this.NotifyActivating();
            RemainingValue -= 1;
            yield return new UpgradeCardAction(toUpgrade);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
        }
    }
}
