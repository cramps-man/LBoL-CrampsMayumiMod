using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaDistractionDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaSharpshooter);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaSharpshooter);
        }
    }

    [EntityLogic(typeof(OptionHaniwaDistractionDef))]
    public sealed class OptionHaniwaSharpshooter: ModFrontlineOptionCard
    {
        public override int SelectRequireArcher => 1;
        public override Type CardTypeToSpawn => typeof(HaniwaSharpshooter);
    }
}
