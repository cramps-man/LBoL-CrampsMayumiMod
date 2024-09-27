using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaBodyguardDef : ModFrontlineOptionCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaBodyguard);
        }
    }

    [EntityLogic(typeof(OptionHaniwaBodyguardDef))]
    public sealed class OptionHaniwaBodyguard: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 1;
        public override Type CardTypeToSpawn => typeof(HaniwaBodyguard);
    }
}
