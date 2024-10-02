using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaGeneralDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaGeneral);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaGeneral);
        }
    }

    [EntityLogic(typeof(OptionHaniwaGeneralDef))]
    public sealed class OptionHaniwaGeneral: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 3;
        public override Type CardTypeToSpawn => typeof(HaniwaGeneral);
    }
}
