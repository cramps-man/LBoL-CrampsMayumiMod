using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class OptionFrontlineCommanderDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(FrontlineCommander);
        public override IdContainer GetId()
        {
            return nameof(OptionFrontlineCommander);
        }
        public override CardConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.Rarity = Rarity.Rare;
            return config;
        }
    }

    [EntityLogic(typeof(OptionFrontlineCommanderDef))]
    public sealed class OptionFrontlineCommander: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 3;
        public override int SelectRequireArcher => 3;
        public override int SelectRequireCavalry => 3;
        public override Type CardTypeToSpawn => typeof(FrontlineCommander);
    }
}
