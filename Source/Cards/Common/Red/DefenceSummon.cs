using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Source.Utils;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class DefenceSummonDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DefenceSummon);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Any = 1, Red = 1 };
            cardConfig.Block = 10;
            cardConfig.UpgradedBlock = 14;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(DefenceSummonDef))]
    public sealed class DefenceSummon : Card
    {
        public override bool CanUse => HaniwaUtils.HasAnyHaniwa(base.Battle.Player);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Card> cards = new List<Card>();
            foreach (var type in HaniwaFrontlineUtils.CommonSummonTypes)
            {
                var c = Library.CreateCard(type) as ModFrontlineOptionCard;
                c.SetBattle(base.Battle);
                if (c.FulfilsRequirement)
                    cards.Add(c);
            }
            cards.Shuffle(base.BattleRng);
            cards.RemoveRange(Value2, cards.Count - Value2);
            var selectInteraction = new SelectCardInteraction(0, Value1, cards);
            yield return new InteractionAction(selectInteraction);

            foreach (var card in selectInteraction.SelectedCards)
            {
                OptionCard optionCard = card as OptionCard;
                if (optionCard == null)
                    continue;

                foreach (BattleAction battleAction in optionCard.TakeEffectActions())
                {
                    yield return battleAction;
                }
            }

            yield return DefenseAction();
        }
    }
}
