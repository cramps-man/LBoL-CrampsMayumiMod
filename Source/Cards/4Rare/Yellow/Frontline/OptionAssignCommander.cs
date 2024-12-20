using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class OptionAssignCommanderDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(AssignCommander);
        public override IdContainer GetId()
        {
            return nameof(OptionAssignCommander);
        }
        public override CardConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.Rarity = Rarity.Rare;
            return config;
        }
    }

    [EntityLogic(typeof(OptionAssignCommanderDef))]
    public sealed class OptionAssignCommander: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 3;
        public override int SelectRequireArcher => 3;
        public override int SelectRequireCavalry => 3;
        public override Type CardTypeToSpawn => typeof(AssignCommander);
    }
}
