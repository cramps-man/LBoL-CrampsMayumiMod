using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaSpyDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaSpy);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaSpy);
        }
    }

    [EntityLogic(typeof(OptionHaniwaSpyDef))]
    public sealed class OptionHaniwaSpy: ModFrontlineOptionCard
    {
        public override int SelectRequireCavalry => 2;
        public override Type CardTypeToSpawn => typeof(HaniwaSpy);
    }
}
