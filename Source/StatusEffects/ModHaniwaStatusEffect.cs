using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLMod.StatusEffects.Abilities;

namespace LBoLMod.StatusEffects
{
    public abstract class ModHaniwaStatusEffect: StatusEffect
    {
        private int AdditionalMaxLevel
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                if (base.Battle.Player.TryGetStatusEffect<MaxHaniwaUpSe>(out var additionalHaniwa))
                    return additionalHaniwa.Level;
                return 0;
            }
        }
        private int MaxLevel => 10 + AdditionalMaxLevel;

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
