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
    public sealed class AssignEndTurnBlockDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignEndTurnBlockSe);
        }
    }

    [EntityLogic(typeof(AssignEndTurnBlockDef))]
    public sealed class AssignEndTurnBlockSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            ReactOwnerEvent(Battle.Player.TurnEnded, OnPlayerTurnEnded);
        }

        private IEnumerable<BattleAction> OnPlayerTurnEnded(UnitEventArgs args)
        {
            int numAssignStatuses = Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect).Count();
            yield return new CastBlockShieldAction(args.Unit, Level * numAssignStatuses, 0);
        }
    }
}
