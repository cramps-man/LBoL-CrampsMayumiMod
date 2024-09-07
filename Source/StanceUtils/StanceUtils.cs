using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLMod.StatusEffects;
using Unit = LBoL.Core.Units.Unit;

namespace LBoLMod
{
    public static class StanceUtils
    {
        public static BattleAction ApplyStance<T>(Card card) where T : ModStanceStatusEffect
        {
            if (!card.Battle.Player.HasStatusEffect<T>() && !card.Battle.Player.HasStatusEffect<Downtime>())
                return card.BuffAction<T>();
            return null;
        }

        public static BattleAction RemoveStance<T>(Unit unit) where T : ModStanceStatusEffect
        {
            if (!unit.HasStatusEffect<T>())
            {
                return null;
            }
            var se = unit.GetStatusEffect<T>();
            if (se.Preserved) {
                return null;
            }
            return new RemoveStatusEffectAction(se);
        }

        public static BattleAction ForceRemoveStance<T>(Unit unit) where T : ModStanceStatusEffect
        {
            if (!unit.HasStatusEffect<T>())
            {
                return null;
            }
            var se = unit.GetStatusEffect<T>();
            return new RemoveStatusEffectAction(se);
        }

        public static BattleAction RemoveDowntime(Unit unit)
        {
            if (!unit.HasStatusEffect<Downtime>())
            {
                return null;
            }
            var se = unit.GetStatusEffect<Downtime>();
            return new RemoveStatusEffectAction(se);
        }

        public static bool DoesPlayerHavePreservedStance(PlayerUnit player)
        {
            if (player.HasStatusEffect<PowerStance>())
            {
                var se = player.GetStatusEffect<PowerStance>();
                if (se.Preserved)
                    return true;
            }
            if (player.HasStatusEffect<FocusStance>())
            {
                var se = player.GetStatusEffect<FocusStance>();
                if (se.Preserved)
                    return true;
            }
            if (player.HasStatusEffect<CalmStance>())
            {
                var se = player.GetStatusEffect<CalmStance>();
                if (se.Preserved)
                    return true;
            }
            return false;
        }

        public static BattleAction RemoveNonPreservedStance(Unit unit)
        {
            if (unit.HasStatusEffect<PowerStance>())
            {
                var se = unit.GetStatusEffect<PowerStance>();
                if (!se.Preserved)
                    return new RemoveStatusEffectAction(se);
            }
            if (unit.HasStatusEffect<FocusStance>())
            {
                var se = unit.GetStatusEffect<FocusStance>();
                if (!se.Preserved)
                    return new RemoveStatusEffectAction(se);
            }
            if (unit.HasStatusEffect<CalmStance>())
            {
                var se = unit.GetStatusEffect<CalmStance>();
                if (!se.Preserved)
                    return new RemoveStatusEffectAction(se);
            }
            return null;
        }

        public static void PreserveCurrentStance(PlayerUnit player)
        {
            if (player.HasStatusEffect<PowerStance>())
            {
                var se = player.GetStatusEffect<PowerStance>();
                if (!se.Preserved)
                {
                    se.Preserved = true;
                    se.NotifyActivating();
                }
                return;
            }
            if (player.HasStatusEffect<FocusStance>())
            {
                var se = player.GetStatusEffect<FocusStance>();
                if (!se.Preserved)
                {
                    se.Preserved = true;
                    se.NotifyActivating();
                }
                return;
            }
            if (player.HasStatusEffect<CalmStance>())
            {
                var se = player.GetStatusEffect<CalmStance>();
                if (!se.Preserved)
                {
                    se.Preserved = true;
                    se.NotifyActivating();
                }
                return;
            }
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
