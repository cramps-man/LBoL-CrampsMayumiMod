using LBoL.Core.Units;
using LBoLMod.Exhibits;
using LBoLMod.StatusEffects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Utils
{
    public static class HaniwaUtils
    {
        public static bool DoesPlayerHavePreservedHaniwa(PlayerUnit player)
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

        public static void PreserveAllCurrentHaniwa(PlayerUnit player)
        {
            PreserveSpecifiedHaniwa<ArcherHaniwa>(player);
            PreserveSpecifiedHaniwa<CavalryHaniwa>(player);
            PreserveSpecifiedHaniwa<FencerHaniwa>(player);
        }

        public static List<ModHaniwaStatusEffect> GetAllHaniwa(PlayerUnit player)
        {
            var haniwas = new List<ModHaniwaStatusEffect>();
            if (player.HasStatusEffect<ArcherHaniwa>())
            {
                haniwas.Add(player.GetStatusEffect<ArcherHaniwa>());
            }
            if (player.HasStatusEffect<CavalryHaniwa>())
            {
                haniwas.Add(player.GetStatusEffect<CavalryHaniwa>());
            }
            if (player.HasStatusEffect<FencerHaniwa>())
            {
                haniwas.Add(player.GetStatusEffect<FencerHaniwa>());
            }
            return haniwas;
        }

        public static void PreserveOldestHaniwa(PlayerUnit player)
        {
            var haniwa = GetAllHaniwa(player);
            if (haniwa.Count > 0)
            {
                var oldestAge = haniwa.Where(s => !s.Preserved).Max(s => s.AgeCounter);
                foreach (var s in haniwa.Where(s => s.AgeCounter == oldestAge))
                {
                    s.Preserved = true;
                }
            }
        }

        public static void PreserveSpecifiedHaniwa<T>(PlayerUnit player) where T : ModHaniwaStatusEffect
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

        public static int TotalHaniwaLevel(PlayerUnit player)
        {
            int total = 0;
            var stances = GetAllHaniwa(player);
            stances.ForEach(st => total += st.Level);
            return total;
        }

        public static bool IsLevelFulfilled(PlayerUnit player, HaniwaActionType actionType, int fencerRequired = 0, int archerRequired = 0, int cavalryRequired = 0)
        {
            return IsLevelFulfilled<FencerHaniwa>(player, fencerRequired, actionType)
                && IsLevelFulfilled<ArcherHaniwa>(player, archerRequired, actionType)
                && IsLevelFulfilled<CavalryHaniwa>(player, cavalryRequired, actionType);
        }

        public static bool IsLevelFulfilled<T>(PlayerUnit player, int requiredLevel, HaniwaActionType actionType) where T : ModHaniwaStatusEffect
        {
            return IsLevelFulfilled(player, typeof(T), requiredLevel, actionType);
        }

        public static bool IsLevelFulfilled(PlayerUnit player, Type haniwaType, int requiredLevel, HaniwaActionType actionType)
        {
            if (requiredLevel == 0)
                return true;
            if (!player.HasStatusEffect(haniwaType))
                return false;
            if (actionType == HaniwaActionType.Sacrifice && player.HasExhibit<ExhibitB>())
                requiredLevel = Math.Max(requiredLevel - 1, 0);

            var se = player.GetStatusEffect(haniwaType);
            return se.Level >= requiredLevel;
        }

        public static int GetHaniwaLevel<T>(PlayerUnit player) where T : ModHaniwaStatusEffect
        {
            if (!player.HasStatusEffect<T>())
                return 0;
            return player.GetStatusEffect<T>().Level;
        }

        public static bool HasAnyHaniwa(PlayerUnit player)
        {
            return GetHaniwaLevel<FencerHaniwa>(player) > 0
                || GetHaniwaLevel<ArcherHaniwa>(player) > 0
                || GetHaniwaLevel<CavalryHaniwa>(player) > 0;
        }
    }
}
