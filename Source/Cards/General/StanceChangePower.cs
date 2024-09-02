using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.PlayerUnits;
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
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StanceChangePowerDef))]
    public sealed class StanceChangePower:OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            yield return StanceApplier.StanceApplier.ApplyStance<PowerStance>(this);
            yield break;
        }
    }
}
