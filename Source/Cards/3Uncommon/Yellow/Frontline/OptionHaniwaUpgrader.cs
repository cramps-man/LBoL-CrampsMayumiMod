using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaUpgraderDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaUpgrader);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaUpgrader);
        }
        public override CardConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.Rarity = Rarity.Uncommon;
            return config;
        }
    }

    [EntityLogic(typeof(OptionHaniwaUpgraderDef))]
    public sealed class OptionHaniwaUpgrader: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 2;
        public override Type CardTypeToSpawn => typeof(HaniwaUpgrader);
    }
}
