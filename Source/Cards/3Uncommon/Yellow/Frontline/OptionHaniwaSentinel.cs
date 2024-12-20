using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaSentinelDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaSentinel);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaSentinel);
        }
        public override CardConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.Rarity = Rarity.Uncommon;
            return config;
        }
    }

    [EntityLogic(typeof(OptionHaniwaSentinelDef))]
    public sealed class OptionHaniwaSentinel: ModFrontlineOptionCard
    {
        public override int SelectRequireArcher => 2;
        public override Type CardTypeToSpawn => typeof(HaniwaSentinel);
    }
}
