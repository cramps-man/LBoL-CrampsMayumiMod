using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class BlitzCommandDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BlitzCommand);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.AllEnemies;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Command) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Command) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BlitzCommandDef))]
    public sealed class BlitzCommand : Card
    {
        public List<Card> CardsInHand => HaniwaFrontlineUtils.GetCommandableCards(base.Battle.HandZone.ToList(), this);
        public List<Card> CardsInPlay => HaniwaFrontlineUtils.GetCommandableCards(base.Battle.HandZone.Concat(base.Battle.DrawZone).Concat(base.Battle.DiscardZone).ToList(), this);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Card> cardsToCommand = IsUpgraded ? CardsInPlay : CardsInHand;
            foreach (var battleAction in HaniwaFrontlineUtils.ExecuteOnPlayActions(cardsToCommand, base.Battle, consumeRemainingValue: true))
            {
                yield return battleAction;
            }
        }
    }
}
