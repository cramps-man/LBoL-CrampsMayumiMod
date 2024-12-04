using LBoL.Core.Units;
using LBoLMod.StatusEffects;
using System;
using System.Collections.Generic;

namespace LBoLMod.Utils
{
    public static class HaniwaUtils
    {
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
            //move this to an ability, which ill make later
            /*if (actionType == HaniwaActionType.Sacrifice && player.HasExhibit<ExhibitB>())
                requiredLevel = Math.Max(requiredLevel - 1, 0);*/

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
