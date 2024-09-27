using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaAttackerDef : ModFrontlineOptionCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaAttacker);
        }
    }

    [EntityLogic(typeof(OptionHaniwaAttackerDef))]
    public sealed class OptionHaniwaAttacker: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 1;
        public override Type CardTypeToSpawn => typeof(HaniwaAttacker);
    }
}
