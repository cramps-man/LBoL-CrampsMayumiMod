using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class GarrisonArcherDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(GarrisonArcher);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.HideMesuem = true;
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Cost = new ManaGroup() { Any = 0 };
            cardConfig.Value1 = 4;
            cardConfig.UpgradedValue1 = 6;
            cardConfig.Keywords = Keyword.Exile | Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice), nameof(Watchtower) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice), nameof(Watchtower) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(GarrisonArcherDef))]
    public sealed class GarrisonArcher:Card
    {
        public int ArcherSacrificed => 2;
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled<ArcherHaniwa>(base.Battle.Player, ArcherSacrificed, HaniwaActionType.Sacrifice);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, archerToLose: ArcherSacrificed);
            yield return new ApplyStatusEffectAction<Watchtower>(base.Battle.Player, Value1);
        }
    }
}
