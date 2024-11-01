using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaMonkDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaMonk);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaMonk);
        }
    }

    [EntityLogic(typeof(OptionHaniwaMonkDef))]
    public sealed class OptionHaniwaMonk: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 2;
        public override Type CardTypeToSpawn => typeof(HaniwaMonk);
    }
}
