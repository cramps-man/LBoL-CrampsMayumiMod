using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaSpyDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaSpy);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaSpy);
        }
        public override CardConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.Rarity = Rarity.Uncommon;
            return config;
        }
    }

    [EntityLogic(typeof(OptionHaniwaSpyDef))]
    public sealed class OptionHaniwaSpy: ModFrontlineOptionCard
    {
        public override int SelectRequireCavalry => 2;
        public override Type CardTypeToSpawn => typeof(HaniwaSpy);
    }
}
