using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Source.StatusEffects.Keywords;
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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Preserve) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Preserve) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(PreserveStanceDef))]
    public sealed class PreserveStance:Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (this.IsUpgraded) 
            {
                StanceUtils.PreserveAllCurrentStances(base.Battle.Player);
            }
            else
            {
                StanceUtils.PreserveOldestStance(base.Battle.Player);
            }
            yield break;
        }
    }
}
