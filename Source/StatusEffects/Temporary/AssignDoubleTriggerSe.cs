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
    public sealed class AssignDoubleTriggerSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignDoubleTriggerSe);
        }
    }

    [EntityLogic(typeof(AssignDoubleTriggerSeDef))]
    public sealed class AssignDoubleTriggerSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.AssignEffectTriggered, this.OnAssignTriggered);
        }

        private IEnumerable<BattleAction> OnAssignTriggered(AssignTriggerEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            foreach (var battleAction in args.BeforeBattleActions)
            {
                yield return battleAction;
                if (base.Battle.BattleShouldEnd)
                    yield break;
            };
            
            foreach (var battleAction in args.BattleActions)
            {
                yield return battleAction;
                if (base.Battle.BattleShouldEnd)
                    yield break;
            };
            
            foreach (var battleAction in args.AfterBattleActions)
            {
                yield return battleAction;
                if (base.Battle.BattleShouldEnd)
                    yield break;
            };

            if (Level > 1)
                Level--;
            else
                yield return new RemoveStatusEffectAction(this);
        }
    }
}
