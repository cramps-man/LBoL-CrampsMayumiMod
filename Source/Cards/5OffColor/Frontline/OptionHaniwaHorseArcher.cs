using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaHorseArcherDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaHorseArcher);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaHorseArcher);
        }

        public override CardConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Green };
            return config;
        }
    }

    [EntityLogic(typeof(OptionHaniwaHorseArcherDef))]
    public sealed class OptionHaniwaHorseArcher: ModFrontlineOptionCard
    {
        public override int SelectRequireArcher => 2;
        public override int SelectRequireCavalry => 2;
        public override Type CardTypeToSpawn => typeof(HaniwaHorseArcher);
    }
}
