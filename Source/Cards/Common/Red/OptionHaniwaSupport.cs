using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaSupportDef : ModFrontlineOptionCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaSupport);
        }
    }

    [EntityLogic(typeof(OptionHaniwaSupportDef))]
    public sealed class OptionHaniwaSupport: ModFrontlineOptionCard
    {
        public override int SelectRequireCavalry => 1;
        public override Type CardTypeToSpawn => typeof(HaniwaSupport);
    }
}
