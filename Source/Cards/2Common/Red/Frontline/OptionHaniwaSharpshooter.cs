using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaSharpshooterDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaSharpshooter);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaSharpshooter);
        }
    }

    [EntityLogic(typeof(OptionHaniwaSharpshooterDef))]
    public sealed class OptionHaniwaSharpshooter: ModFrontlineOptionCard
    {
        public override int SelectRequireArcher => 1;
        public override Type CardTypeToSpawn => typeof(HaniwaSharpshooter);
    }
}
