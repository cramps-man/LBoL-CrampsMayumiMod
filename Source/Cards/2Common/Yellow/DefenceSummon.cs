using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects.Localization;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class DefenceSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DefenceSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 1, White = 1 };
            cardConfig.Block = 8;
            cardConfig.UpgradedBlock = 12;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 5;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa), nameof(LoyaltyProtectionSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice), nameof(Haniwa), nameof(LoyaltyProtectionSe) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaUpgrader), nameof(HaniwaExploiter) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaUpgrader), nameof(HaniwaExploiter) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(DefenceSummonDef))]
    public sealed class DefenceSummon : Card
    {
        public override bool CanUse => HaniwaUtils.HasAnyHaniwa(base.Battle.Player);
        public override string CantUseMessage => LocSe.RequiresHaniwa();
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override Interaction Precondition()
        {
            List<ModFrontlineOptionCard> cards = HaniwaFrontlineUtils.GetAllOptionCards(base.Battle, checkSacrificeRequirement: true);
            return new SelectCardInteraction(0, Value1, cards)
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
            yield return DefenseAction();
            yield return BuffAction<LoyaltyProtectionSe>(Value2);
        }
    }
}
