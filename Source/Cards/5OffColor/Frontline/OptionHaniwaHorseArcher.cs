using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionHaniwaHorseArcherDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(HaniwaHorseArcher);
        public override IdContainer GetId()
        {
            return nameof(OptionHaniwaHorseArcher);
        }
    }

    [EntityLogic(typeof(OptionHaniwaHorseArcherDef))]
    public sealed class OptionHaniwaHorseArcher: ModFrontlineOptionCard
    {
        public override int SelectRequireArcher => 2;
        public override int SelectRequireCavalry => 2;
        public override Type CardTypeToSpawn => typeof(HaniwaHorseArcher);
    }
}
