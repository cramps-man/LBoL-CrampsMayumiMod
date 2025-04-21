using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects.Localization;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class BasicSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BasicSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1 };
            cardConfig.Value1 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BasicSummonDef))]
    public sealed class BasicSummon : Card
    {
        public override bool CanUse => HaniwaUtils.HasAnyHaniwa(base.Battle.Player);
        public override string CantUseMessage => LocSe.RequiresHaniwa();
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override Interaction Precondition()
        {
            List<ModFrontlineOptionCard> cards = HaniwaFrontlineUtils.GetOptionCards(HaniwaFrontlineUtils.CommonOptionTypes, base.Battle, Value1, true);
            return new SelectCardInteraction(1, 1, cards)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction interaction))
                yield break;

            foreach (var action in HaniwaFrontlineUtils.CardsSummon(((SelectCardInteraction)precondition).SelectedCards))
            {
                yield return action;
            };
        }
    }
}
