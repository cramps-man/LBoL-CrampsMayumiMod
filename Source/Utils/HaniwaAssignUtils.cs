using HarmonyLib;
using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLMod.Cards;
using LBoLMod.StatusEffects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Utils
{
    public static class HaniwaAssignUtils
    {
        public readonly static List<Type> OnColorAssignCardTypes = new List<Type>()
        {
            typeof(ArcherPrepVolley),
            typeof(CavalrySupplies),
            typeof(FencerBuildBarricade),
            typeof(BuildWatchtower),
            typeof(CavalryRush),
            typeof(CavalryScout),
            typeof(ChargeAttack),
            typeof(ArcherPrepDebuff),
            typeof(FencerPrepCounter)
        };
        
        public static List<Type> GetAssignCardTypes(PlayerUnit player, bool checkOffColor = true)
        {
            if (checkOffColor)
            {
                if (player.GameRun.BaseMana.HasColor(ManaColor.Blue))
                    return OnColorAssignCardTypes.AddItem(typeof(ArcherPrepFrostArrow)).ToList();
            }
            return OnColorAssignCardTypes;
        }
        public static List<Card> CreateAssignOptionCards(PlayerUnit player, bool includePaused = true, bool includePermanent = true)
        {
            List<Card> list = new List<Card>();
            foreach (ModAssignStatusEffect s in player.StatusEffects.Where(s => s is ModAssignStatusEffect))
            {
                ModAssignOptionCard c = Library.CreateCard<ModAssignOptionCard>();
                c.CardName = s.Name;
                c.CardText = s.Description;
                c.StatusEffect = s;
                bool toAdd = true;
                if (!includePaused && s.IsPaused)
                    toAdd = false;
                if (!includePermanent && s.IsPermanent)
                    toAdd = false;
                if (toAdd)
                    list.Add(c);
            };
            return list;
        }
    }
}
