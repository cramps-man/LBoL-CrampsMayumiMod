using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class VigorousCommandDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(VigorousCommand);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Green };
            cardConfig.Cost = new ManaGroup() { Green = 1, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 0 };
            cardConfig.Value1 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Command) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Command) };
            cardConfig.RelativeKeyword = Keyword.Overdraft;
            cardConfig.UpgradedRelativeKeyword = Keyword.Overdraft;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(VigorousCommandDef))]
    public sealed class VigorousCommand : ModMayumiCard
    {
        public override Interaction Precondition()
        {
            List<Card> list = HaniwaFrontlineUtils.GetCommandableCards(base.Battle.HandZone.ToList(), this, false);
            return new SelectHandInteraction(1, Value1, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectHandInteraction selectInteraction))
                yield break;

            yield return new CommandAction(selectInteraction.SelectedCards.ToList(), selector, true, Name);
            yield return new LockRandomTurnManaAction(selectInteraction.SelectedCards[0].Cost.Amount);
        }
    }
}
