using HarmonyLib;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoL.EntityLib.Exhibits.Shining;
using LBoLEntitySideloader.Utils;
using LBoLMod.Cards;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Assign;
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
                bool hasBlue = player.GameRun.BaseMana.HasColor(ManaColor.Blue);
                bool hasBlankCard = player.HasExhibit<KongbaiKapai>();
                if (hasBlue || hasBlankCard)
                    return OnColorAssignCardTypes.AddItem(typeof(ArcherPrepFrostArrow)).ToList();
            }
            return OnColorAssignCardTypes;
        }
        public static List<ModAssignOptionCard> CreateAssignOptionCards(PlayerUnit player, bool includePermanent = true)
        {
            List<ModAssignOptionCard> list = new List<ModAssignOptionCard>();
            foreach (ModAssignStatusEffect s in player.StatusEffects.Where(s => s is ModAssignStatusEffect))
            {
                ModAssignOptionCard c = Library.CreateCard<ModAssignOptionCard>();
                c.CardName = s.Name;
                c.CardText = s.Description;
                c.StatusEffect = s;
                if (c.StatusEffect is AssignArcherPrepFrostArrow)
                {
                    var blueConfig = c.Config.Copy();
                    blueConfig.Colors = new List<ManaColor>() { ManaColor.Blue };
                    c.Config = blueConfig;
                }
                bool toAdd = true;
                if (!includePermanent && s.IsPermanent)
                    toAdd = false;
                if (toAdd)
                    list.Add(c);
            };
            return list;
        }

        public static IEnumerable<BattleAction> GetRandomAssignBuffs(BattleController battle, int fencerCount = 0, int archerCount = 0, int cavalryCount = 0, bool manualStack = true)
        {
            Dictionary<string, int> haniwaCount = new Dictionary<string, int>()
            {
                { "fencer", fencerCount },
                { "archer", archerCount },
                { "cavalry", cavalryCount },
            };
            bool continueRandomizing = true;
            while (continueRandomizing)
            {
                var allOptions = GetAssignCardTypes(battle.Player).Select(Library.CreateCard).Cast<ModAssignCard>().ToList();
                while (true)
                {
                    var randomSelection = allOptions.SampleOrDefault(battle.GameRun.BattleRng);
                    if (randomSelection == null)
                    {
                        continueRandomizing = false;
                        break;
                    }
                    allOptions.Remove(randomSelection);
                    if (randomSelection.FencerAssigned > haniwaCount["fencer"])
                        continue;
                    if (randomSelection.ArcherAssigned > haniwaCount["archer"])
                        continue;
                    if (randomSelection.CavalryAssigned > haniwaCount["cavalry"])
                        continue;
                    randomSelection.SetBattle(battle);
                    randomSelection.ManualStack = manualStack;
                    var assignBuffAction = randomSelection.BuffAction(randomSelection.AssignStatusType, level: randomSelection.StartingTaskLevel, count: randomSelection.StartingCardCounter);
                    yield return assignBuffAction;
                    if (assignBuffAction is ApplyStatusEffectAction sea && sea.Args.Effect is ModAssignStatusEffect mase)
                        mase.JustApplied = false; //false to prevent next card play not ticking down count, since buff was created not with a card play, flag isnt toggled until 2 card plays, so manually set to bypass that 
                    haniwaCount["fencer"] -= randomSelection.FencerAssigned;
                    haniwaCount["archer"] -= randomSelection.ArcherAssigned;
                    haniwaCount["cavalry"] -= randomSelection.CavalryAssigned;
                    break;
                }
            }
        }
    }
}
