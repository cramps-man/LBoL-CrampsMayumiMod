using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLMod.Exhibits;
using LBoLMod.Source.StatusEffects.Keywords;
using LBoLMod.Source.StatusEffects.Stances;
using LBoLMod.StatusEffects;
using System.Collections.Generic;
using System.Linq;
using Unit = LBoL.Core.Units.Unit;

namespace LBoLMod
{
    public static class StanceUtils
    {
        public static IEnumerable<BattleAction> ApplyStance<T>(PlayerUnit player, int level = 1) where T : ModStanceStatusEffect
        {
            if (player.HasStatusEffect<Downtime>())
            {
                var se = player.GetStatusEffect<Downtime>();
                se.NotifyActivating();
                yield break;
            }
            yield return new ApplyStatusEffectAction<T>(player, level);
            //exhibit A's specialty is to not have stances be removed when applying others
            if (player.HasExhibit<ExhibitA>())
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

        public static void PreserveAllCurrentStances(PlayerUnit player)
        {
            PreserveSpecifiedStance<PowerStance>(player);
            PreserveSpecifiedStance<FocusStance>(player);
            PreserveSpecifiedStance<CalmStance>(player);
        }

        public static void PreserveOldestStance(PlayerUnit player)
        {
            var stances = new List<ModStanceStatusEffect>();
            if (player.HasStatusEffect<PowerStance>())
            {
                var se = player.GetStatusEffect<PowerStance>();
                if (!se.Preserved)
                {
                    stances.Add(se);
                }
            }
            if (player.HasStatusEffect<FocusStance>())
            {
                var se = player.GetStatusEffect<FocusStance>();
                if (!se.Preserved)
                {
                    stances.Add(se);
                }
            }
            if (player.HasStatusEffect<CalmStance>())
            {
                var se = player.GetStatusEffect<CalmStance>();
                if (!se.Preserved)
                {
                    stances.Add(se);
                }
            }
            if (stances.Count > 0)
            {
                var oldestAge = stances.Max(s => s.AgeCounter);
                foreach (var s in stances.Where(s => s.AgeCounter == oldestAge))
                {
                    s.Preserved = true;
                    s.NotifyActivating();
                    s.NotifyChanged();
                }
            }
        }

        public static void PreserveSpecifiedStance<T>(PlayerUnit player) where T : ModStanceStatusEffect
        {
            if (player.HasStatusEffect<T>())
            {
                var se = player.GetStatusEffect<T>();
                if (!se.Preserved)
                {
                    se.Preserved = true;
                    se.NotifyActivating();
                    se.NotifyChanged();
                }
            }
        }

        public static bool isStanceFulfilled<T>(PlayerUnit player) where T : ModStanceStatusEffect
        {
            if (player.HasStatusEffect<T>())
                return true;
            if (player.HasStatusEffect<Dexterity>())
                return true;
            return false;
        }

        public static BattleAction RemoveDexterityIfNeeded<T>(PlayerUnit player, int requiredLevel = 1) where T : ModStanceStatusEffect
        {
            if (!player.HasStatusEffect<Dexterity>())
                return null;
            if (player.HasStatusEffect<T>())
            {
                var se = player.GetStatusEffect<T>();
                if (se.Level >= requiredLevel)
                    return null;
            }
            var dex = player.GetStatusEffect<Dexterity>();
            if (dex.Level > 1)
            {
                dex.Level -= 1;
                return null;
            }
            return new RemoveStatusEffectAction(dex);
        }

        public static int TotalStanceLevel(PlayerUnit player)
        {
            int total = 0;
            if (player.HasStatusEffect<PowerStance>())
            {
                var se = player.GetStatusEffect<PowerStance>();
                total += se.Level;
            }
            if (player.HasStatusEffect<FocusStance>())
            {
                var se = player.GetStatusEffect<FocusStance>();
                total += se.Level;
            }
            if (player.HasStatusEffect<CalmStance>())
            {
                var se = player.GetStatusEffect<CalmStance>();
                total += se.Level;
            }
            return total;
        }
    }
}
