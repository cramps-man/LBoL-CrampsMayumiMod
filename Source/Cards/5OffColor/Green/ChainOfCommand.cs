using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class ChainOfCommandDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ChainOfCommand);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Green };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 3, Any = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 0 };
            cardConfig.RelativeCards = new List<string>() { nameof(FrontlineCommander), nameof(AssignCommander) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(FrontlineCommander), nameof(AssignCommander) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ChainOfCommandDef))]
    public sealed class ChainOfCommand : Card
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent(base.Battle.BattleStarted, this.OnBattleStarted);
        }

        private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
        {
            if (IsUpgraded)
                yield return new PlayCardAction(this);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<ChainOfCommandSe>();
        }
    }
}
