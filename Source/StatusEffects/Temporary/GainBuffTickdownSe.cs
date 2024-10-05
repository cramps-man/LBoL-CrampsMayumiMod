using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
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
    }

    [EntityLogic(typeof(GainBuffTickdownSeDef))]
    public sealed class GainBuffTickdownSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent(base.Battle.Player.StatusEffectAdded, this.OnStatusEffectAdded);
            base.ReactOwnerEvent(base.Battle.Player.TurnEnded, this.OnPlayerTurnEnded);
        }

        private IEnumerable<BattleAction> OnPlayerTurnEnded(UnitEventArgs args)
        {
            yield return new RemoveStatusEffectAction(this);
        }

        private void OnStatusEffectAdded(StatusEffectApplyEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                return;
            if (args.Effect.Type != StatusEffectType.Positive)
                return;
            if (args.AddResult != StatusEffectAddResult.Added)
                return;

            var assigns = base.Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect);
            foreach (ModAssignStatusEffect assign in assigns)
            {
                assign.Tickdown(Level);
            }
        }
    }
}
