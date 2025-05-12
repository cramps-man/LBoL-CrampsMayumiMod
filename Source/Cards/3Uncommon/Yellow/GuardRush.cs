using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class GuardRushDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(GuardRush);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaBodyguard), nameof(HaniwaHorseArcher) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaBodyguard), nameof(HaniwaHorseArcher) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(GuardRushDef))]
    public sealed class GuardRush : ModMayumiCard
    {
        public override bool CanUse
        {
            get
            {
                var player = base.Battle.Player;
                return HaniwaUtils.IsLevelFulfilled(player, HaniwaActionType.Sacrifice, Value2, Value2, Value2);
            }
        }
        public override string CantUseMessage => HaniwaUtils.CantUseMessages(base.Battle.Player, HaniwaActionType.Sacrifice, Value2, Value2, Value2);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, Value2, Value2, Value2);
            List<Card> frontlines = new List<Card>() { Library.CreateCard<HaniwaBodyguard>(), Library.CreateCard<HaniwaBodyguard>(), Library.CreateCard<HaniwaHorseArcher>() };
            yield return new AddCardsToDrawZoneAction(frontlines, DrawZoneTarget.Top);
            yield return new DrawManyCardAction(Value1);
        }
    }
}
