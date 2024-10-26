using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaCommanderDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaCommander);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaCommander);
        }
    }

    [EntityLogic(typeof(OptionHaniwaCommanderDef))]
    public sealed class OptionHaniwaCommander: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 3;
        public override Type CardTypeToSpawn => typeof(HaniwaCommander);
    }
}
