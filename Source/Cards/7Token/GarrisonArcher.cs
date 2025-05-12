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
            cardConfig.Keywords = Keyword.Exile | Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice), nameof(Watchtower) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice), nameof(Watchtower) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(GarrisonArcherDef))]
    public sealed class GarrisonArcher : ModMayumiCard
    {
        public int ArcherLvl0 => 1;
        public int ArcherLvl1 => IsUpgraded ? 3 : 2;
        public int ArcherLvl2 => IsUpgraded ? 6 : 4;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int archerLevel = HaniwaUtils.GetHaniwaLevel<ArcherHaniwa>(base.Battle.Player);
            int watchtowerGain = ArcherLvl0;
            if (archerLevel >= 2)
            {
                watchtowerGain = ArcherLvl2;
                yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, archerToLose: 2);
            }
            else if (archerLevel == 1)
            {
                watchtowerGain = ArcherLvl1;
                yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, archerToLose: 1);
            }
            yield return new ApplyStatusEffectAction<Watchtower>(base.Battle.Player, watchtowerGain);
        }
    }
}
