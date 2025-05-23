﻿using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Assign;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects.Localization;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class BuildWatchtowerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BuildWatchtower);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 5;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Sacrifice), nameof(Watchtower) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Sacrifice), nameof(Watchtower) };
            cardConfig.RelativeCards = new List<string>() { nameof(GarrisonArcher) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(GarrisonArcher)+"+" };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BuildWatchtowerDef))]
    public sealed class BuildWatchtower : ModAssignCard
    {
        public override int FencerAssigned => 3;
        public int SacrificeArcherRequired
        {
            get
            {
                if (base.Battle == null)
                    return 2;
                if (base.Battle.Player.HasStatusEffect(AssignStatusType) || base.Battle.Player.HasStatusEffect<Watchtower>())
                    return 0;
                return 2;
            }
        }
        public override int FencerRequired
        {
            get
            {
                if (base.Battle == null)
                    return base.FencerRequired;
                if (base.Battle.Player.HasStatusEffect<Watchtower>())
                    return 0;
                return base.FencerRequired;
            }
        }
        public override int StartingCardCounter => IsUpgraded ? 8 : 12;
        public override int StartingTaskLevel => IsUpgraded ? 30 : 15;
        public override Type AssignStatusType => typeof(AssignBuildWatchtower);
        public override bool CanUse {
            get
            {
                var player = base.Battle.Player;
                if (player.HasStatusEffect<Watchtower>())
                    return true;
                return base.CanUse && HaniwaUtils.IsLevelFulfilled<ArcherHaniwa>(player, SacrificeArcherRequired, HaniwaActionType.Sacrifice);
            }
        }
        public override string CantUseMessage
        {
            get
            {
                var player = base.Battle.Player;
                string message = "";
                if (!HaniwaUtils.IsLevelFulfilled<FencerHaniwa>(player, FencerRequired, HaniwaActionType.Assign))
                    message += LocSe.NeedMoreFencer();
                if (!HaniwaUtils.IsLevelFulfilled<ArcherHaniwa>(player, SacrificeArcherRequired, HaniwaActionType.Sacrifice))
                    message += LocSe.NeedMoreArcher();
                return message;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            var player = base.Battle.Player;
            if (!player.HasStatusEffect<Watchtower>())
            {
                yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, archerToLose: SacrificeArcherRequired);
                foreach (var item in base.Actions(selector, consumingMana, precondition))
                {
                    yield return item;
                };
            }
            else
                yield return new AddCardsToHandAction(Library.CreateCard<GarrisonArcher>(IsUpgraded));
        }
    }
}
