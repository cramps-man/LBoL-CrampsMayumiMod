using LBoL.Base;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignCostTriggerSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCostTriggerSe);
        }
    }

    [EntityLogic(typeof(AssignCostTriggerSeDef))]
    public sealed class AssignCostTriggerSe: StatusEffect
    {
        public ManaGroup IncreasedCost => ManaGroup.Anys(Level);
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent(base.Battle.Player.StatusEffectAdding, this.OnStatusEffectAdding);
        }

        private void OnStatusEffectAdding(StatusEffectApplyEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                return;
            if (args.Effect is ModAssignStatusEffect assignStatus)
            {
                assignStatus.Level += Level;
            }
        }
    }
}
