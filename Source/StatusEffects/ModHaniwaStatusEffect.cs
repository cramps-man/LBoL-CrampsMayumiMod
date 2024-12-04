using LBoL.Core.StatusEffects;
using LBoL.Core.Units;

namespace LBoLMod.StatusEffects
{
    public abstract class ModHaniwaStatusEffect: StatusEffect
    {
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
        }

        public override bool Stack(StatusEffect other)
        {
            base.Stack(other);
            if (Level > MaxLevel)
                Level = MaxLevel;
            return true;
        }
    }
}
