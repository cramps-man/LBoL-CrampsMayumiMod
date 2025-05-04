using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Debuffs;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class AssignmentOrderDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignmentOrder);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.HideMesuem = true;
            cardConfig.IsUpgradable = false;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Type = CardType.Skill;
            cardConfig.Value1 = 1;
            cardConfig.Keywords = Keyword.Exile | Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentMarkSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentMarkSe) };
            cardConfig.RelativeCards = new List<string>() { nameof(AssignmentOrderHaste), nameof(AssignmentOrderDelay), nameof(AssignmentOrderGuard), nameof(AssignmentOrderRecall), nameof(AssignmentOrderMark) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(AssignmentOrderHaste), nameof(AssignmentOrderDelay), nameof(AssignmentOrderGuard), nameof(AssignmentOrderRecall), nameof(AssignmentOrderMark) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignmentOrderDef))]
    public sealed class AssignmentOrder : Card
    {
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override Interaction Precondition()
        {
            var selectList = new List<Card>();
            if (base.Battle.Player.StatusEffects.Where(se => se is ModAssignStatusEffect).Any())
            {
                selectList.Add(Library.CreateCard<AssignmentOrderHaste>());
                selectList.Add(Library.CreateCard<AssignmentOrderDelay>());
                selectList.Add(Library.CreateCard<AssignmentOrderGuard>());
                selectList.Add(Library.CreateCard<AssignmentOrderRecall>());
            }
            selectList.Add(Library.CreateCard<AssignmentOrderMark>());
            foreach (var card in selectList)
                card.SetBattle(Battle);
            return new MiniSelectCardInteraction(selectList)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is MiniSelectCardInteraction interaction))
            {
                yield break;
            }

            if (interaction.SelectedCard is OptionCard optionCard)
            {
                if (optionCard is AssignmentOrderMark aom)
                    aom.selector = selector;
                foreach (BattleAction battleAction in optionCard.TakeEffectActions())
                {
                    yield return battleAction;
                }
            }
        }
    }
}
