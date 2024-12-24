using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
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
    public sealed class GainBuffTickdownSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(GainBuffTickdownSe);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(GainBuffTickdownSeDef))]
    public sealed class GainBuffTickdownSe: StatusEffect
    {
        public int TotalFirepower => Count / FirepowerScaling;
        public int FirepowerScaling => 3;
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent(ModGameEvents.AssignEffectTriggered, this.OnAssignTriggered);
            base.HandleOwnerEvent(base.Battle.Player.StatusEffectAdded, this.OnStatusEffectAdded);
            base.ReactOwnerEvent(base.Battle.Player.TurnStarted, this.OnPlayerTurnStarted);
        }

        private void OnAssignTriggered(AssignTriggerEventArgs args)
        {
            Count++;
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
        {
            if (Count >= FirepowerScaling)
            {
                base.NotifyActivating();
                yield return BuffAction<TempFirepower>(TotalFirepower);
            }
            yield return new RemoveStatusEffectAction(this);
        }

        private void OnStatusEffectAdded(StatusEffectApplyEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                return;
            if (args.Effect.Type != StatusEffectType.Positive)
                return;

            var assigns = base.Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect);
            foreach (ModAssignStatusEffect assign in assigns)
            {
                assign.Tickdown(Level);
            }
        }
    }
}
