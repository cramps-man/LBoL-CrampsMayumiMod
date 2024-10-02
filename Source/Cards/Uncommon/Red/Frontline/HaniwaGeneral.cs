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

namespace LBoLMod.Cards
{
    public sealed class HaniwaGeneralDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaGeneral);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            //cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 20;
            cardConfig.UpgradedDamage = 25;
            cardConfig.Value1 = 7;
            cardConfig.UpgradedValue1 = 10;
            cardConfig.Keywords = Keyword.Retain | Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaGeneralDef))]
    public sealed class HaniwaGeneral : ModFrontlineCard
    {
        public int HaniwaDamage
        {
            get
            {
                if (base.Battle != null)
                    return base.Battle.HandZoneAndPlayArea.Where(c => c is ModFrontlineCard).Count() * Value1;
                return 0;
            }
        }
        public override int AdditionalDamage => HaniwaDamage;
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

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
        }
    }
}
