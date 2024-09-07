using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public abstract class ModStanceStatusEffect: StatusEffect
    {
        protected bool preserved = false;
        public bool Preserved
        {
            get
            {
                return preserved;
            }
        }

        protected override void OnAdding(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStart));
        }

        private IEnumerable<BattleAction> onPlayerTurnStart(UnitEventArgs args)
        {
            if (Preserved)
            {
                preserved = false;
            }
            yield break;
        }
    }
}
