using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class PreserveStanceDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PreserveStance);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.TargetType = TargetType.Self;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Green };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 9 };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(PreserveStanceDef))]
    public sealed class PreserveStance:Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!StanceUtils.DoesPlayerHavePreservedStance(base.Battle.Player))
                StanceUtils.PreserveCurrentStance(base.Battle.Player);
            yield break;
        }
    }
}
