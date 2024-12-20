using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaAssassinDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaAssassin);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaAssassin);
        }

        public override CardConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            return config;
        }
    }

    [EntityLogic(typeof(OptionHaniwaAssassinDef))]
    public sealed class OptionHaniwaAssassin: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 3;
        public override Type CardTypeToSpawn => typeof(HaniwaAssassin);
    }
}
