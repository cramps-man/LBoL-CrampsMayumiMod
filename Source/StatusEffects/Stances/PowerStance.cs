using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;

namespace LBoLMod.StatusEffects
{
    public sealed class PowerStanceDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PowerStance);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.IsStackable = false;
            statusConfig.HasLevel = false;
            return statusConfig;
        }
    }

    public sealed class PowerStance: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            this.React(new ApplyStatusEffectAction<Firepower>(unit, 1));
            this.React(StanceApplier.StanceApplier.RemoveStance<FocusStance>(unit));
            this.React(StanceApplier.StanceApplier.RemoveStance<CalmStance>(unit));
        }

        protected override void OnRemoved(Unit unit)
        {
            this.React(new ApplyStatusEffectAction<FirepowerNegative>(unit, 1));
        }
    }
}
