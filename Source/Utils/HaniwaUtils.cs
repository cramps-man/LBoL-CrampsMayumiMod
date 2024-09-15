using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLMod.Source.StatusEffects.Stances;
using LBoLMod.StatusEffects;
using System.Collections.Generic;
using System.Linq;
using Unit = LBoL.Core.Units.Unit;

namespace LBoLMod
{
    public static class HaniwaUtils
    {
        public static IEnumerable<BattleAction> ApplyStance<T>(PlayerUnit player, int level = 1) where T : ModHaniwaStatusEffect
        {
            if (player.HasStatusEffect<Downtime>())
            {
                var se = player.GetStatusEffect<Downtime>();
                se.NotifyActivating();
                yield break;
            }
            yield return new ApplyStatusEffectAction<T>(player, level);
            //test without removing stances at all
            //exhibit A's specialty is to not have stances be removed when applying others
            /*if (player.HasExhibit<ExhibitA>())
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
            }*/
        }

        public static BattleAction ForceApplyStance<T>(PlayerUnit player, int level = 1) where T : ModHaniwaStatusEffect
        {
            return new ApplyStatusEffectAction<T>(player, level);
        }

        public static BattleAction RemoveStance<T>(Unit unit) where T : ModHaniwaStatusEffect
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

        public static BattleAction ForceRemoveStance<T>(Unit unit) where T : ModHaniwaStatusEffect
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
            if (player.HasStatusEffect<ArcherHaniwa>())
            {
                var se = player.GetStatusEffect<ArcherHaniwa>();
                if (se.Preserved)
                    return true;
            }
            if (player.HasStatusEffect<CavalryHaniwa>())
            {
                var se = player.GetStatusEffect<CavalryHaniwa>();
                if (se.Preserved)
                    return true;
            }
            if (player.HasStatusEffect<FencerHaniwa>())
            {
                var se = player.GetStatusEffect<FencerHaniwa>();
                if (se.Preserved)
                    return true;
            }
            return false;
        }

        public static void PreserveAllCurrentStances(PlayerUnit player)
        {
            PreserveSpecifiedStance<ArcherHaniwa>(player);
            PreserveSpecifiedStance<CavalryHaniwa>(player);
            PreserveSpecifiedStance<FencerHaniwa>(player);
        }

        public static List<ModHaniwaStatusEffect> GetAllStances(PlayerUnit player)
        {
            var stances = new List<ModHaniwaStatusEffect>();
            if (player.HasStatusEffect<ArcherHaniwa>())
            {
                stances.Add(player.GetStatusEffect<ArcherHaniwa>());
            }
            if (player.HasStatusEffect<CavalryHaniwa>())
            {
                stances.Add(player.GetStatusEffect<CavalryHaniwa>());
            }
            if (player.HasStatusEffect<FencerHaniwa>())
            {
                stances.Add(player.GetStatusEffect<FencerHaniwa>());
            }
            return stances;
        }

        public static void PreserveOldestStance(PlayerUnit player)
        {
            var stances = GetAllStances(player);
            if (stances.Count > 0)
            {
                var oldestAge = stances.Where(s => !s.Preserved).Max(s => s.AgeCounter);
                foreach (var s in stances.Where(s => s.AgeCounter == oldestAge))
                {
                    s.Preserved = true;
                }
            }
        }

        public static void PreserveSpecifiedStance<T>(PlayerUnit player) where T : ModHaniwaStatusEffect
        {
            if (player.HasStatusEffect<T>())
            {
                var se = player.GetStatusEffect<T>();
                if (!se.Preserved)
                {
                    se.Preserved = true;
                }
            }
        }

        public static bool isStanceFulfilled<T>(PlayerUnit player) where T : ModHaniwaStatusEffect
        {
            if (player.HasStatusEffect<T>())
                return true;
            if (player.HasStatusEffect<Dexterity>())
                return true;
            return false;
        }

        public static BattleAction RemoveDexterityIfNeeded<T>(PlayerUnit player, int requiredLevel = 1) where T : ModHaniwaStatusEffect
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
            var stances = GetAllStances(player);
            stances.ForEach(st => total += st.Level);
            return total;
        }
    }
}
