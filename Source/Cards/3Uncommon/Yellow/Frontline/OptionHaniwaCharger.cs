using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaChargerDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaCharger);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaCharger);
        }
    }

    [EntityLogic(typeof(OptionHaniwaChargerDef))]
    public sealed class OptionHaniwaCharger: ModFrontlineOptionCard
    {
        public override int SelectRequireCavalry => 2;
        public override Type CardTypeToSpawn => typeof(HaniwaCharger);
    }
}
