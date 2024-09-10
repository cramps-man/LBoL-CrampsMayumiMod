using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class EnterCalmDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(EnterCalm);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Green };
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 9 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Mana = new ManaGroup() { Red = 1, Green = 1 };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(EnterCalmDef))]
    public sealed class EnterCalm : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (var item in StanceUtils.ApplyStance<CalmStance>(Battle.Player))
            {
                yield return item;
            };
            yield return new GainManaAction(Mana);
        }
    }
}
