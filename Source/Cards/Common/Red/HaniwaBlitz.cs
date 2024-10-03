using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class HaniwaBlitzDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HaniwaBlitz);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1, Any = 1 };
            cardConfig.Damage = 12;
            cardConfig.UpgradedDamage = 15;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaBlitzDef))]
    public sealed class HaniwaBlitz : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card c) => c != this && c is ModFrontlineCard && !(c is HaniwaCommander)).ToList();
            return new SelectHandInteraction(0, Value1, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (!(precondition is SelectHandInteraction selectInteraction))
                yield break;

            foreach (var item in selectInteraction.SelectedCards)
            {
                foreach (var action in item.GetActions(selector, consumingMana, null, new List<DamageAction>(), false))
                {
                    if (base.Battle.BattleShouldEnd)
                        yield break;
                    yield return action;
                }
            }
        }
    }
}
