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
    public sealed class AssignExtraTimeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignExtraTime);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1 };
            cardConfig.Value1 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignExtraTimeDef))]
    public sealed class AssignExtraTime : Card
    {
        public override Interaction Precondition()
        {
            return new MiniSelectCardInteraction(HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player, true));
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                var c = ((MiniSelectCardInteraction)precondition).SelectedCard as ModAssignOptionCard;
                c.StatusEffect.Level++;
            }
            yield break;
        }
    }
}
