using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignArcherPrepVolleyDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignArcherPrepVolley);
        }
    }

    [EntityLogic(typeof(AssignArcherPrepVolleyDef))]
    public sealed class AssignArcherPrepVolley : ModAssignStatusEffect
    {
        public int Damage => 5;
        protected override IEnumerable<BattleAction> OnAssignmentDone()
        {
            for (int i = 0; i < Level; i++)
            {
                yield return new DamageAction(Owner, base.Battle.RandomAliveEnemy, DamageInfo.Attack(Damage));
            }
            yield return HaniwaUtils.ForceGainHaniwa<ArcherHaniwa>(base.Battle.Player, HaniwaAssigned);
        }
    }
}
