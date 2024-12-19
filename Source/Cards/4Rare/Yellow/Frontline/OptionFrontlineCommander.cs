using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionFrontlineCommanderDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(FrontlineCommander);
        public override IdContainer GetId()
        {
            return nameof(OptionFrontlineCommander);
        }
    }

    [EntityLogic(typeof(OptionFrontlineCommanderDef))]
    public sealed class OptionFrontlineCommander: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 3;
        public override int SelectRequireArcher => 3;
        public override int SelectRequireCavalry => 3;
        public override Type CardTypeToSpawn => typeof(FrontlineCommander);
    }
}
