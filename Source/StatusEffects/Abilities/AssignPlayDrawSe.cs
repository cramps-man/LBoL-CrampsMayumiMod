using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignPlayDrawSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignPlayDrawSe);
        }
    }

    [EntityLogic(typeof(AssignPlayDrawSeDef))]
    public sealed class AssignPlayDrawSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent(base.Battle.Player.StatusEffectAdding, this.OnStatusEffectAdding);
        }

        private void OnStatusEffectAdding(StatusEffectApplyEventArgs args)
        {
            if (args.Effect is AssignmentBonusSe)
            {
                base.NotifyActivating();
                args.Level += Level;
                args.Effect.Level += Level;
            }
        }
    }
}
