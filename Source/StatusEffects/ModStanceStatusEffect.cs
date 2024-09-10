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
            set
            {
                preserved = value;
            }
        }
        protected int ageCounter = 0;
        public int AgeCounter
        {
            get
            {
                return ageCounter;
            }
        }

        protected override void OnAdding(Unit unit)
        {
            if (Level > 3)
                Level = 3;
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStart));
            base.ReactOwnerEvent<StatusEffectApplyEventArgs>(base.Battle.Player.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.onStatusApplied));
        }

        private IEnumerable<BattleAction> onStatusApplied(StatusEffectApplyEventArgs args)
        {
            if (args.Effect is ModStanceStatusEffect)
            {
                ageCounter++;
            }
            yield break;
        }

        private IEnumerable<BattleAction> onPlayerTurnStart(UnitEventArgs args)
        {
            if (Preserved)
            {
                base.NotifyChanged();
                preserved = false;
            }
            yield break;
        }

        public override bool Stack(StatusEffect other)
        {
            base.Stack(other);
            if (Level > 3)
                Level = 3;
            return true;
        }
    }
}
