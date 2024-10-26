using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class MassExtraTriggerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MassExtraTrigger);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 3;
            cardConfig.Value2 = 2;
            cardConfig.Cost = new ManaGroup() { Red = 3 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(MassExtraTriggerDef))]
    public sealed class MassExtraTrigger : Card
    {
        public override Interaction Precondition()
        {
            return new SelectCardInteraction(0, 1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player));
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (ModAssignOptionCard card in ((SelectCardInteraction)precondition).SelectedCards)
            {
                card.StatusEffect.IncreaseExtraTrigger(Value2);
                List<StatusEffect> removedAssignBuffs = base.Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect && s != card.StatusEffect).ToList();
                foreach (StatusEffect statusEffect in removedAssignBuffs)
                {
                    card.StatusEffect.IncreaseExtraTrigger(statusEffect.Level);
                    yield return new RemoveStatusEffectAction(statusEffect);
                }
            }
            yield return new DrawManyCardAction(Value1);
        }
    }
}
