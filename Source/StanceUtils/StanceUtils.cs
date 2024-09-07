using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLMod.StatusEffects;

namespace LBoLMod
{
    public static class StanceUtils
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

        public static bool IsPowerStanceConditionFulfilled(PlayerUnit player)
        {
            if (player.HasStatusEffect<PowerStance>() || player.HasStatusEffect<BoostedPowerStance>())
            {
                return true;
            }
            return false;
        }

        public static bool IsFocusStanceConditionFulfilled(PlayerUnit player)
        {
            if (player.HasStatusEffect<FocusStance>() || player.HasStatusEffect<BoostedFocusStance>())
            {
                return true;
            }
            return false;
        }

        public static bool IsCalmStanceConditionFulfilled(PlayerUnit player)
        {
            if (player.HasStatusEffect<CalmStance>() || player.HasStatusEffect<BoostedCalmStance>())
            {
                return true;
            }
            return false;
        }
    }
}
