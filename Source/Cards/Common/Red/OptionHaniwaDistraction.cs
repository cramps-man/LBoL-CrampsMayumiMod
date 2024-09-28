using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaDistractionDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaDistraction);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaDistraction);
        }
    }

    [EntityLogic(typeof(OptionHaniwaDistractionDef))]
    public sealed class OptionHaniwaDistraction: ModFrontlineOptionCard
    {
        public override int SelectRequireArcher => 1;
        public override Type CardTypeToSpawn => typeof(HaniwaDistraction);
    }
}
