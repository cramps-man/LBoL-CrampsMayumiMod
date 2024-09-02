using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;

namespace LBoLMod.StanceApplier
{
    public static class StanceApplier
    {
        public static BattleAction ApplyStance<T>(Card card) where T : StatusEffect
        {
            if (!card.Battle.Player.HasStatusEffect<T>())
                return card.BuffAction<T>();
            return null;
        }

        public static BattleAction RemoveStance<T>(Unit unit) where T : StatusEffect
        {
            if (unit.HasStatusEffect<T>())
                return new RemoveStatusEffectAction(unit.GetStatusEffect<T>());
            return null;
        }
    }
}
