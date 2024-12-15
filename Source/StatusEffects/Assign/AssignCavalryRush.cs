using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignCavalryRushDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCavalryRush);
        }
    }

    [EntityLogic(typeof(AssignCavalryRushDef))]
    public sealed class AssignCavalryRush : ModAssignStatusEffect
    {
        public DamageInfo TotalDamage => CardDamage.MultiplyBy(Level);
        public ManaGroup TotalMana => CardMana * (Level / CardValue1);
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield return new DamageAction(Owner, base.Battle.LowestHpEnemy, TotalDamage);
            if (!onTurnStart)
                yield return new GainManaAction(TotalMana);
        }
    }
}
