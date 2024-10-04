using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects.Temporary;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignPauseBlockDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignPauseBlock);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1, White = 1 };
            cardConfig.Block = 8;
            cardConfig.UpgradedBlock = 12;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign), nameof(Pause) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(Pause) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignPauseBlockDef))]
    public sealed class AssignPauseBlock : Card
    {
        public override Interaction Precondition()
        {
            return new MiniSelectCardInteraction(HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player, false));
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                var c = ((MiniSelectCardInteraction)precondition).SelectedCard as ModAssignOptionCard;
                yield return new AssignPauseAction(c.StatusEffect);
            }
            yield return BuffAction<PauseBlock>(Block.Block);
        }
    }
}
