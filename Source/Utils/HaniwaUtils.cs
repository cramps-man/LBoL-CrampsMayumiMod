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
        public static IEnumerable<BattleAction> GainHaniwa<T>(PlayerUnit player, int level = 1) where T : ModHaniwaStatusEffect
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

        public static BattleAction ForceGainHaniwa<T>(PlayerUnit player, int level = 1) where T : ModHaniwaStatusEffect
        {
            return new ApplyStatusEffectAction<T>(player, level);
        }

        public static BattleAction RemoveHaniwa<T>(Unit unit) where T : ModHaniwaStatusEffect
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

        public static BattleAction ForceRemoveHaniwa<T>(Unit unit) where T : ModHaniwaStatusEffect
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

        public static bool IsHaniwaFulfilled<T>(PlayerUnit player) where T : ModHaniwaStatusEffect
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

        public static int TotalHaniwaLevel(PlayerUnit player)
        {
            int total = 0;
            var stances = GetAllHaniwa(player);
            stances.ForEach(st => total += st.Level);
            return total;
        }

        public static BattleAction SacrificeHaniwa<T>(PlayerUnit player, int levelToLose) where T : ModHaniwaStatusEffect
        {
            if (!player.HasStatusEffect<T>())
                return null;

            var se = player.GetStatusEffect<T>();
            if (se.Level > levelToLose)
            {
                se.Level -= levelToLose;
                return null;
            }
            return new RemoveStatusEffectAction(se);
        }
        
        public static bool IsLevelFulfilled<T>(PlayerUnit player, int requiredLevel = 1) where T : ModHaniwaStatusEffect
        {
            if (!player.HasStatusEffect<T>())
                return false;

            var se = player.GetStatusEffect<T>();
            return se.Level >= requiredLevel;
        }
    }
}
