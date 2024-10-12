using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class FrozenHaniwaDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrozenHaniwa);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.AllEnemies;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Blue };
            cardConfig.Damage = 16;
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 1;
            cardConfig.Keywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(FrostArmor), nameof(Cold) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(FrostArmor), nameof(Cold) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FrozenHaniwaDef))]
    public sealed class FrozenHaniwa : ModFrontlineCard
    {
        public Card OriginalCard { get; set; }
        public override int AdditionalDamage => base.UpgradeCounter.GetValueOrDefault();
        protected override bool IncludeUpgradesInRemainingValue => true;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(base.Battle.CardUsed, this.OnCardUsed);
            base.ReactBattleEvent(base.Battle.Player.TurnEnded, this.OnTurnEnded);
        }

        private IEnumerable<BattleAction> OnTurnEnded(UnitEventArgs args)
        {
            OriginalCard.UpgradeCounter = base.UpgradeCounter;
            yield return new TransformCardAction(this, OriginalCard);
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;

            int usedCardIndex = args.Card.HandIndexWhenPlaying;
            int index = base.Battle.HandZone.FindIndexOf(c => c == this);
            if (usedCardIndex == index || usedCardIndex == index + 1)
            {
                base.NotifyActivating();
                yield return BuffAction<FrostArmor>(Value2);
                if (RemainingValue > 0)
                {
                    yield return DebuffAction<Cold>(base.Battle.RandomAliveEnemy);
                    RemainingValue -= 1;
                    base.NotifyChanged();
                }
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;
            foreach (var item in DebuffAction<Cold>(selector.GetEnemies(Battle)))
            {
                yield return item;
            };
        }
    }
}
