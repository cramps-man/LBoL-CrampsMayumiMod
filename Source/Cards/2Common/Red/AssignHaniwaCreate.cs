using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignHaniwaCreateDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignHaniwaCreate);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Value2 = 1;
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.Keywords = Keyword.Replenish;
            cardConfig.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignHaniwaCreateDef))]
    public sealed class AssignHaniwaCreate : Card
    {
        public override Interaction Precondition()
        {
            return new SelectCardInteraction(0, 1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player));
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (((SelectCardInteraction)precondition).SelectedCards.Empty())
            {
                yield return new GainHaniwaAction(1, 1, 1);
                yield break;
            }

            foreach (ModAssignOptionCard optionCard in ((SelectCardInteraction)precondition).SelectedCards)
            {
                optionCard.StatusEffect.Count += Value2;
                int fencerGain = 0;
                int archerGain = 0;
                int cavalryGain = 0;
                if (optionCard.StatusEffect.CardFencerAssigned > 0)
                    fencerGain += optionCard.StatusEffect.CardFencerAssigned + Value1;
                if (optionCard.StatusEffect.CardArcherAssigned > 0)
                    archerGain += optionCard.StatusEffect.CardArcherAssigned + Value1;
                if (optionCard.StatusEffect.CardCavalryAssigned > 0)
                    cavalryGain += optionCard.StatusEffect.CardCavalryAssigned + Value1;
                yield return new GainHaniwaAction(fencerGain, archerGain, cavalryGain);
            }
        }
    }
}
