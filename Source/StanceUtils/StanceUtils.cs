using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLMod.StatusEffects;
using System.Collections.Generic;
using Unit = LBoL.Core.Units.Unit;

namespace LBoLMod
{
    public static class StanceUtils
    {
        public static IEnumerable<BattleAction> ApplyStance<T>(PlayerUnit player, int level = 1) where T : ModStanceStatusEffect
        {
            if (player.HasStatusEffect<Downtime>())
                yield break;
            //during ultimate skill A, dont add or remove anything
            if (player.HasStatusEffect<PowerStance>() && player.HasStatusEffect<FocusStance>() && player.HasStatusEffect<CalmStance>())
                yield break;
            if (typeof(T) == typeof(PowerStance))
            {
                yield return RemoveStance<FocusStance>(player);
                yield return RemoveStance<CalmStance>(player);
            }
            else if (typeof(T) == typeof(FocusStance))
            {
                yield return RemoveStance<PowerStance>(player);
                yield return RemoveStance<CalmStance>(player);
            }
            else if (typeof(T) == typeof(CalmStance))
            {
                yield return RemoveStance<FocusStance>(player);
                yield return RemoveStance<PowerStance>(player);
            }
            yield return new ApplyStatusEffectAction<T>(player, level);
        }

        public static BattleAction ForceApplyStance<T>(PlayerUnit player, int level = 1) where T : ModStanceStatusEffect
        {
            return new ApplyStatusEffectAction<T>(player, level);
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

        public static bool isStanceFulfilled<T>(PlayerUnit player) where T : ModStanceStatusEffect
        {
            if (player.HasStatusEffect<T>())
                return true;
            return false;
        }
    }
}
