using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.StatusEffects.Temporary
{
    public sealed class PauseBlockDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PauseBlock);
        }
    }

    [EntityLogic(typeof(PauseBlockDef))]
    public sealed class PauseBlock: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(base.Battle.Player.TurnEnded, this.OnPlayerTurnEnded);
        }

        private IEnumerable<BattleAction> OnPlayerTurnEnded(UnitEventArgs args)
        {
            int numAssignStatuses = base.Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect).Count();
            yield return new CastBlockShieldAction(args.Unit, Level * numAssignStatuses, 0);
            yield return new RemoveStatusEffectAction(this);
        }
    }
}
