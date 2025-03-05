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
                base.NotifyActivating();
                if (Level > args.Effect.Level)
                {
                    Level -= args.Effect.Level;
                    args.CancelBy(this);
                }
                else
                {
                    if (Level == args.Effect.Level)
                        args.CancelBy(this);
                    else
                        args.Effect.Level -= Level;
                    yield return new RemoveStatusEffectAction(this);
                }
            }
            else if (args.Effect.HasDuration)
            {
                base.NotifyActivating();
                if (Level > args.Effect.Duration)
                {
                    Level -= args.Effect.Duration;
                    args.CancelBy(this);
                }
                else
                {
                    if (Level == args.Effect.Duration)
                        args.CancelBy(this);
                    else
                        args.Effect.Duration -= Level;
                    yield return new RemoveStatusEffectAction(this);
                }
            }
        }
    }
}
