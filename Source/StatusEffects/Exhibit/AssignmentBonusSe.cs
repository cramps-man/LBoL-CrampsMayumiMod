using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignmentBonusSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignmentBonusSe);
        }
    }

    [EntityLogic(typeof(AssignmentBonusSeDef))]
    public sealed class AssignmentBonusSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.AssignEffectTriggered, this.OnAssignTriggered);
        }

        private IEnumerable<BattleAction> OnAssignTriggered(AssignTriggerEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (args.OnTurnStart)
                yield break;
            if (base.Battle.HandZone.Count == base.Battle.MaxHand)
                yield break;

            base.NotifyActivating();
            yield return new DrawCardAction();
            yield return ConsumeBuff();
        }

        public BattleAction ConsumeBuff()
        {
            if (Level <= 1)
                return new RemoveStatusEffectAction(this);
            else
                Level--;
            return null;
        }
    }
}
