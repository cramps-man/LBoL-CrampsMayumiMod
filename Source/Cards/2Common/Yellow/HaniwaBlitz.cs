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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.Damage = 12;
            cardConfig.UpgradedDamage = 15;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Command) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Command) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HaniwaBlitzDef))]
    public sealed class HaniwaBlitz : Card
    {
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
        public override Interaction Precondition()
        {
            List<Card> list = HaniwaFrontlineUtils.GetCommandableCards(base.Battle, this);
            return new SelectHandInteraction(0, Value1, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (!(precondition is SelectHandInteraction selectInteraction))
                yield break;

            foreach (var battleAction in HaniwaFrontlineUtils.ExecuteOnPlayActions(selectInteraction.SelectedCards.ToList(), base.Battle, selector, true))
            {
                yield return battleAction;
            }
        }
    }
}
