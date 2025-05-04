using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignmentOrderHasteDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignmentOrderHaste);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.HideMesuem = true;
            cardConfig.IsUpgradable = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignmentOrderHasteDef))]
    public sealed class AssignmentOrderHaste : OptionCard
    {
        public int TaskLevelGain => 3;
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            var assignInteraction = new SelectCardInteraction(0, Value1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player))
            {
                Description = InteractionTitle
            };
            yield return new InteractionAction(assignInteraction);
            foreach (ModAssignOptionCard assignCard in assignInteraction.SelectedCards)
            {
                assignCard.StatusEffect.Count -= Value2;
                assignCard.StatusEffect.Level += TaskLevelGain;
                assignCard.StatusEffect.NotifyActivating();
            }
        }
    }
}
