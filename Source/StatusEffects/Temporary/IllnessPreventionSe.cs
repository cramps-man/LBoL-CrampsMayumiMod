using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class IllnessPreventionSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(IllnessPreventionSe);
        }
    }

    [EntityLogic(typeof(IllnessPreventionSeDef))]
    public sealed class IllnessPreventionSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(base.Battle.Player.StatusEffectAdding, this.OnPlayerStatusEffectAdding);
        }

        private IEnumerable<BattleAction> OnPlayerStatusEffectAdding(StatusEffectApplyEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (args.Effect.Type != LBoL.Base.StatusEffectType.Negative)
                yield break;

            if (args.Effect.HasLevel)
            {
                if (args.Effect.Level > 1)
                    args.Effect.Level -= 1;
                else
                    args.CancelBy(this);

                base.NotifyActivating();
                if (Level > 1)
                    Level -= 1;
                else
                    yield return new RemoveStatusEffectAction(this);
            }
            else if (args.Effect.HasDuration)
            {
                if (args.Effect.Duration > 1)
                    args.Effect.Duration -= 1;
                else
                    args.CancelBy(this);

                base.NotifyActivating();
                if (Level > 1)
                    Level -= 1;
                else
                    yield return new RemoveStatusEffectAction(this);
            }
        }
    }
}
