﻿using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects.Localization;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class DarknessSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DarknessSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Black };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Cost = new ManaGroup() { Black = 1, Any = 2 };
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Sacrifice) };
            cardConfig.RelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport), nameof(HaniwaUpgrader), nameof(HaniwaExploiter), nameof(HaniwaSpy), nameof(HaniwaMonk), nameof(HaniwaCharger), nameof(HaniwaSentinel) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(HaniwaAttacker), nameof(HaniwaBodyguard), nameof(HaniwaSharpshooter), nameof(HaniwaSupport), nameof(HaniwaUpgrader), nameof(HaniwaExploiter), nameof(HaniwaSpy), nameof(HaniwaMonk), nameof(HaniwaCharger), nameof(HaniwaSentinel) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(DarknessSummonDef))]
    public sealed class DarknessSummon : ModMayumiCard
    {
        public override bool CanUse => HaniwaUtils.HasAnyHaniwa(base.Battle.Player);
        public override string CantUseMessage => LocSe.RequiresHaniwa();
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
                if (action is AddCardsToHandAction addCardsAction)
                {
                    addCardsAction.Args.Cards.Cast<ModFrontlineCard>().ToList().ForEach(card => {
                        card.IsDarknessMode = true;
                    });
                }
                yield return action;
            };
        }
    }
}
