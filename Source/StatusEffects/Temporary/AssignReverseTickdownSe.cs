using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignReverseTickdownSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignReverseTickdownSe);
        }
    }

    [EntityLogic(typeof(AssignReverseTickdownSeDef))]
    public sealed class AssignReverseTickdownSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(base.Battle.Player.TurnEnded, this.OnPlayerTurnEnded);
        }

        private IEnumerable<BattleAction> OnPlayerTurnEnded(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            int totalCounts = base.Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect).Sum(s => s.Count);
            yield return new CastBlockShieldAction(base.Battle.Player, totalCounts, 0);
            yield return new RemoveStatusEffectAction(this);
        }
    }
}
