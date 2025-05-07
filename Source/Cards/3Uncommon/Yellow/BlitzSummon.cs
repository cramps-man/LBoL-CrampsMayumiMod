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
using LBoLMod.StatusEffects.Localization;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class BlitzSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BlitzSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2 };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Command), nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Command), nameof(Sacrifice) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport), nameof(HaniwaUpgrader), nameof(HaniwaExploiter), nameof(HaniwaSpy), nameof(HaniwaMonk), nameof(HaniwaCharger), nameof(HaniwaSentinel) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport), nameof(HaniwaUpgrader), nameof(HaniwaExploiter), nameof(HaniwaSpy), nameof(HaniwaMonk), nameof(HaniwaCharger), nameof(HaniwaSentinel) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BlitzSummonDef))]
    public sealed class BlitzSummon : Card
    {
        public override bool CanUse => HaniwaUtils.HasAnyHaniwa(base.Battle.Player);
        public override string CantUseMessage => LocSe.RequiresHaniwa();
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override Interaction Precondition()
        {
            List<ModFrontlineOptionCard> cards = HaniwaFrontlineUtils.GetAllOptionCards(base.Battle, checkSacrificeRequirement: true);
            return new SelectCardInteraction(1, Value1, cards)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (var action in HaniwaFrontlineUtils.CardsSummon(((SelectCardInteraction)precondition).SelectedCards))
            {
                yield return action;
                if (action is AddCardsToHandAction addCardsAction)
                {
                    yield return new CommandAction(addCardsAction.Args.Cards.ToList(), selector, true, Name);
                }
            };
        }
    }
}
