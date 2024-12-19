using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System;

namespace LBoLMod.Cards
{
    public sealed class OptionAssignCommanderDef : ModFrontlineOptionCardTemplate
    {
        protected override Type CardTypeToSpawn => typeof(AssignCommander);
        public override IdContainer GetId()
        {
            return nameof(OptionAssignCommander);
        }
    }

    [EntityLogic(typeof(OptionAssignCommanderDef))]
    public sealed class OptionAssignCommander: ModFrontlineOptionCard
    {
        public override int SelectRequireFencer => 3;
        public override int SelectRequireArcher => 3;
        public override int SelectRequireCavalry => 3;
        public override Type CardTypeToSpawn => typeof(AssignCommander);
    }
}
