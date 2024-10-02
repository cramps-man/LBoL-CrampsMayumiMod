using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaRetainerDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaRetainer);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaRetainer);
        }
    }

    [EntityLogic(typeof(OptionHaniwaRetainerDef))]
    public sealed class OptionHaniwaRetainer: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 2;
        public override Type CardTypeToSpawn => typeof(HaniwaRetainer);
    }
}
