using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects;
using System.Collections.Generic;
/*
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
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Preserve) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Preserve) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(PreserveStanceDef))]
    public sealed class PreserveStance:Card
    {
        public override bool CanUse
        {
            get
            {
                var player = base.Battle.Player;
                return player.HasStatusEffect<ArcherHaniwa>() || player.HasStatusEffect<CavalryHaniwa>() || player.HasStatusEffect<FencerHaniwa>();
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (this.IsUpgraded) 
            {
                HaniwaUtils.PreserveAllCurrentStances(base.Battle.Player);
            }
            else
            {
                HaniwaUtils.PreserveOldestStance(base.Battle.Player);
            }
            yield break;
        }
    }
}
*/