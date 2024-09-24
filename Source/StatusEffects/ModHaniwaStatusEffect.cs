using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public abstract class ModHaniwaStatusEffect: StatusEffect
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
                NotifyActivating();
                NotifyChanged();
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
        private int MaxLevel
        {
            get
            {
                return 10;
            }
        }

        protected override void OnAdding(Unit unit)
        {
            if (Level > MaxLevel)
                Level = MaxLevel;
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStart));
            base.ReactOwnerEvent<StatusEffectApplyEventArgs>(base.Battle.Player.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.onStatusApplied));
        }

        private IEnumerable<BattleAction> onStatusApplied(StatusEffectApplyEventArgs args)
        {
            if (args.Effect is ModHaniwaStatusEffect)
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
            if (Level > MaxLevel)
                Level = MaxLevel;
            return true;
        }

        public BattleAction TickdownOrRemove()
        {
            if (Preserved)
                return null;
            if (Level > 1)
            {
                Level -= 1;
                return null;
            }
            return new RemoveStatusEffectAction(this);
        }
    }
}
