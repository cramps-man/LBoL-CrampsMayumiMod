using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Cards;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignPlayDrawSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignPlayDrawSe);
        }
    }

    [EntityLogic(typeof(AssignPlayDrawSeDef))]
    public sealed class AssignPlayDrawSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(base.Battle.CardUsed, this.OnCardUsed);
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Battle == null)
                yield break;
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Battle.HandZone.Count == base.Battle.MaxHand)
                yield break;
            if (!base.Battle.Player.TryGetStatusEffect<AssignmentBonusSe>(out AssignmentBonusSe bonus))
                yield break;

            if (args.Card is ModAssignCard)
            {
                base.NotifyActivating();
                bonus.NotifyActivating();
                yield return new DrawManyCardAction(Level);
                yield return bonus.ConsumeBuff();
            }
        }
    }
}
