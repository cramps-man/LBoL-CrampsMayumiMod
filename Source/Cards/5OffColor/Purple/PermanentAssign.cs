using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class PermanentAssignDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PermanentAssign);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Black };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Cost = new ManaGroup() { Red = 1, Black = 1 };
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Permanent) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Permanent) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(PermanentAssignDef))]
    public sealed class PermanentAssign : ModMayumiCard
    {
        public override Interaction Precondition()
        {
            return new SelectCardInteraction(1, Value1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player, includePermanent: false))
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction assignInteraction))
                yield break;

            foreach (ModAssignOptionCard optionCard in assignInteraction.SelectedCards)
            {
                optionCard.StatusEffect.MakePermanent();
            }
        }
    }
}
