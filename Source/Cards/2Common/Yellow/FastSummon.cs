using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Utils;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using LBoLMod.StatusEffects.Localization;

namespace LBoLMod.Cards
{
    public sealed class FastSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FastSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 0 };
            cardConfig.Value1 = 1;
            cardConfig.Keywords = Keyword.Initial | Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Initial | Keyword.Exile | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport), nameof(HaniwaUpgrader), nameof(HaniwaExploiter), nameof(HaniwaSpy), nameof(HaniwaMonk), nameof(HaniwaCharger), nameof(HaniwaSentinel) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport), nameof(HaniwaUpgrader), nameof(HaniwaExploiter), nameof(HaniwaSpy), nameof(HaniwaMonk), nameof(HaniwaCharger), nameof(HaniwaSentinel) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FastSummonDef))]
    public sealed class FastSummon : Card
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
            };
        }
    }
}
