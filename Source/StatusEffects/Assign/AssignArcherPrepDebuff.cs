using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignArcherPrepDebuffDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignArcherPrepDebuff);
        }
    }

    [EntityLogic(typeof(AssignArcherPrepDebuffDef))]
    public sealed class AssignArcherPrepDebuff : ModAssignStatusEffect
    {
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            var accurateDmg = DamageInfo.Attack(CardDamage.Damage, true);
            yield return new DamageAction(Owner, base.Battle.AllAliveEnemies, accurateDmg);
            foreach (var item in DebuffAction<Vulnerable>(Battle.AllAliveEnemies, duration: CardValue1))
            {
                yield return item;
            };
        }
    }
}
