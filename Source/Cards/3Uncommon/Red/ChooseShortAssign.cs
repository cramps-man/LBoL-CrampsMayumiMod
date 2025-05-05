using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class ChooseShortAssignDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ChooseShortAssign);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2 };
            cardConfig.Value1 = 0;
            cardConfig.Value2 = 5;
            cardConfig.UpgradedValue2 = 20;
            cardConfig.Mana = new ManaGroup() { Colorless = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign), nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(Haniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ChooseShortAssignDef))]
    public sealed class ChooseShortAssign : ModAssignCard
    {
        public override bool CanUse => true;
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Card> cards = HaniwaAssignUtils.GetAssignCardTypes(base.Battle.Player).SampleMany(3, base.BattleRng).Select(Library.CreateCard).ToList();
            if (cards.Count == 0)
                yield break;

            MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(cards)
            {
                Description = InteractionTitle
            };
            yield return new InteractionAction(interaction);
            Card selectedCard = interaction.SelectedCard;
            selectedCard.SetBattle(base.Battle);
            if (selectedCard is ModAssignCard assignCard)
            {
                assignCard.ShouldAssignHaniwa = false;
                yield return new ApplyStatusEffectAction(assignCard.AssignStatusType, base.Battle.Player, level: assignCard.StartingTaskLevel + Value2, count: Value1)
                {
                    Args =
                    {
                        Effect =
                        {
                            SourceCard = assignCard
                        }
                    }
                };
            }
            yield return new GainManaAction(Mana);
        }
    }
}
