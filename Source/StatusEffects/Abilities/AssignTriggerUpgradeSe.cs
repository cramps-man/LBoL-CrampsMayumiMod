using LBoL.Base.Extensions;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignTriggerUpgradeSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignTriggerUpgradeSe);
        }
    }

    [EntityLogic(typeof(AssignTriggerUpgradeSeDef))]
    public sealed class AssignTriggerUpgradeSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            ReactOwnerEvent(ModGameEvents.AssignEffectTriggered, this.OnAssignTriggered);
        }

        private IEnumerable<BattleAction> OnAssignTriggered(AssignTriggerEventArgs args)
        {
            base.NotifyActivating();
            yield return new UpgradeCardsAction(base.Battle.HandZone.Where(c => c.CanUpgradeAndPositive).SampleManyOrAll(Level, base.GameRun.BattleRng));
        }
    }
}
