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
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BlitzCommandDef))]
    public sealed class BlitzCommand : Card
    {
        public IEnumerable<Card> CardsInHand => base.Battle.HandZone.Where(c => c is ModFrontlineCard);
        public IEnumerable<Card> CardsInPlay => base.Battle.HandZone.Concat(base.Battle.DrawZone).Concat(base.Battle.DiscardZone).Where(c => c is ModFrontlineCard);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Card> cardsToCommand = IsUpgraded ? CardsInPlay.ToList() : CardsInHand.ToList();
            foreach (var battleAction in HaniwaFrontlineUtils.ExecuteOnPlayActions(cardsToCommand, base.Battle, consumeRemainingValue: true))
            {
                yield return battleAction;
            }
        }
    }
}
