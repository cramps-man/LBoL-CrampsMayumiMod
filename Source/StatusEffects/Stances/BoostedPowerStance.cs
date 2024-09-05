using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;

namespace LBoLMod.StatusEffects
{
    public sealed class BoostedPowerStanceDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BoostedPowerStance);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.IsStackable = false;
            statusConfig.HasLevel = false;
            statusConfig.HasDuration = true;
            statusConfig.DurationDecreaseTiming = LBoL.Base.DurationDecreaseTiming.TurnStart;
            return statusConfig;
        }
    }

    public sealed class BoostedPowerStance: StatusEffect
    {
        protected override void OnAdding(Unit unit)
        {
            this.React(new ApplyStatusEffectAction<Firepower>(unit, 3));
        }

        protected override void OnRemoving(Unit unit)
        {
            this.React(new ApplyStatusEffectAction<FirepowerNegative>(unit, 3));
        }
    }
}
