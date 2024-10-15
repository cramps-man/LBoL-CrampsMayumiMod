using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
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
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Cost = new ManaGroup() { White = 1, Black = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Permanent) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Permanent) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(PermanentAssignDef))]
    public sealed class PermanentAssign : Card
    {
        public override Interaction Precondition()
        {
            return new SelectCardInteraction(1, Value1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player, includePermanent: false));
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
