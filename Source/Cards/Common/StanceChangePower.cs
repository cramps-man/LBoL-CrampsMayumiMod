using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class StanceChangePowerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StanceChangePower);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.RelativeEffects = new List<string>() { nameof(PowerStance) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(PowerStance) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StanceChangePowerDef))]
    public sealed class StanceChangePower:OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            return StanceUtils.ApplyStance<PowerStance>(base.Battle.Player);
        }
    }
}
