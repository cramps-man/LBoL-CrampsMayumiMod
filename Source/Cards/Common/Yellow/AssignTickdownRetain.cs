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
    public sealed class AssignTickdownRetainDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignTickdownRetain);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 4;
            cardConfig.Keywords = Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignTickdownRetainDef))]
    public sealed class AssignTickdownRetain : Card
    {
        public override Interaction Precondition()
        {
            return new MiniSelectCardInteraction(HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player));
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                var c = ((MiniSelectCardInteraction)precondition).SelectedCard as ModAssignOptionCard;
                c.StatusEffect.Tickdown(Value1);
            }
            yield break;
        }
    }
}
