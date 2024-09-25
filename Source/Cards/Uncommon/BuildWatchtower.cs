using LBoL.Base;
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
            cardConfig.TargetType = TargetType.Nobody;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 0 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 4;
            cardConfig.Value2 = 15;
            cardConfig.UpgradedValue2 = 10;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Sacrifice), nameof(Watchtower) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Sacrifice), nameof(Watchtower) };
            cardConfig.RelativeCards = new List<string>() { nameof(GarrisonArcher) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(GarrisonArcher) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BuildWatchtowerDef))]
    public sealed class BuildWatchtower : ModAssignCard
    {
        public override int FencerRequired => 3;
        public int SacrificeArcherRequired => 2;
        public override int StartingCardCounter => Value2;
        public override Type AssignStatusType => typeof(AssignBuildWatchtower);
        public override bool CanUse {
            get
            {
                var player = base.Battle.Player;
                if (player.HasStatusEffect<AssignBuildWatchtower>())
                    return false;
                if (player.HasStatusEffect<Watchtower>())
                    return true;
                return base.CanUse && HaniwaUtils.IsLevelFulfilled<ArcherHaniwa>(player, SacrificeArcherRequired, HaniwaActionType.Sacrifice);
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
                yield return new AddCardsToHandAction(Library.CreateCard<GarrisonArcher>());
        }
    }
}
