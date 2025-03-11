using LBoL.ConfigData;
using LBoL.Core.Battle;
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
    public sealed class AssignSeparationSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignSeparationSe);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasLevel = false;
            return config;
        }
    }

    [EntityLogic(typeof(AssignSeparationSeDef))]
    public sealed class AssignSeparationSe: StatusEffect
    {
        public int AssignmentBonusGain => 1;
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.AssignEffectTriggered, this.OnAssignTriggered);
        }

        private IEnumerable<BattleAction> OnAssignTriggered(AssignTriggerEventArgs args)
        {
            if (base.Battle.Player.StatusEffects.Where(se => se.GetType() == args.TriggeredEffect.GetType()).Count() > 1)
            {
                yield return BuffAction<AssignmentBonusSe>(AssignmentBonusGain);
            }
        }
    }
}
