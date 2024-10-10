using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignChargeAttackDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignChargeAttack);
        }
    }

    [EntityLogic(typeof(AssignChargeAttackDef))]
    public sealed class AssignChargeAttack : ModAssignStatusEffect
    {
        public DamageInfo TotalDamage => CardDamage.IncreaseBy(ChargeDamage);
        public int ChargeDamage { get; set; } = 0;

        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
            base.HandleOwnerEvent(ModGameEvents.AssignEffectTriggered, this.OnAssignTriggered);
        }

        private void OnAssignTriggered(AssignTriggerEventArgs args)
        {
            ChargeDamage += CardValue1;
            Level++;
        }

        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield return new DamageAction(Owner, base.Battle.HighestHpEnemy, CardDamage.IncreaseBy(ChargeDamage));
        }
    }
}
