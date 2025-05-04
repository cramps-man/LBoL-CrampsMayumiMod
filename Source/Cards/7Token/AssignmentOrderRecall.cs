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
    public sealed class AssignmentOrderRecallDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignmentOrderRecall);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.HideMesuem = true;
            cardConfig.IsUpgradable = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign), nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(Haniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignmentOrderRecallDef))]
    public sealed class AssignmentOrderRecall : OptionCard
    {
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
                assignCard.StatusEffect.NotifyActivating();
                foreach (var item in assignCard.StatusEffect.RemoveBuff())
                {
                    yield return item;
                };
                yield return new DrawManyCardAction(Value2);
            }
        }
    }
}
