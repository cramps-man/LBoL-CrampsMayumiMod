using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class HaniwaRetainerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaRetainer);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Block = 10;
            cardConfig.Value1 = 0;
            cardConfig.Value2 = 3;
            cardConfig.UpgradedValue2 = 5;
            cardConfig.Keywords = Keyword.Retain | Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            cardConfig.RelativeKeyword = Keyword.TempRetain;
            cardConfig.UpgradedRelativeKeyword = Keyword.TempRetain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaRetainerDef))]
    public sealed class HaniwaRetainer : ModFrontlineCard
    {
        public int RetainBlock
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                return base.Battle.HandZoneAndPlayArea.Where(c => c.IsRetain || c.IsTempRetain).ToList().Count * Value2;
            }
        }
        public override int AdditionalBlock => RetainBlock;
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
            if (base.Battle.HandZone.Count <= 1)
                yield break;

            this.NotifyActivating();
            int index = base.Battle.HandZone.FindIndexOf(c => c == this);
            if (index != 0 && index != base.Battle.HandZone.Count - 1)
            {
                SetTempRetain(base.Battle.HandZone[index + 1]);
                SetTempRetain(base.Battle.HandZone[index - 1]);
            } 
            else if (index == 0)
            {
                SetTempRetain(base.Battle.HandZone[index + 1]);
            }
            else if (index == base.Battle.HandZone.Count - 1)
            {
                SetTempRetain(base.Battle.HandZone[index - 1]);
            }
            this.NotifyChanged();
        }

        private void SetTempRetain(Card card)
        {
            if (!card.IsTempRetain && !card.IsRetain)
                card.IsTempRetain = true;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
        }
    }
}
