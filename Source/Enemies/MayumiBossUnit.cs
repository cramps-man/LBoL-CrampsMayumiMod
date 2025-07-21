using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.Enemies
{
    public sealed class MayumiBossUnitDef : MayumiBossUnitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MayumiBossUnit);
        }
    }

    [EntityLogic(typeof(MayumiBossUnitDef))]
    public sealed class MayumiBossUnit : EnemyUnit
    {

    }
}
