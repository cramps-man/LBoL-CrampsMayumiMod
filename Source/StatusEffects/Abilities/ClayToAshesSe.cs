using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.Cards;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class ClayToAshesSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ClayToAshesSe);
        }
    }

    [EntityLogic(typeof(ClayToAshesSeDef))]
    public sealed class ClayToAshesSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.SacrificedHaniwa, this.OnHaniwaSacrificed);
            base.ReactOwnerEvent(base.Battle.CardExiled, this.OnCardExiled);
        }

        private IEnumerable<BattleAction> OnCardExiled(CardEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (args.Card is ModFrontlineCard)
            {
                base.NotifyActivating();
                yield return BuffAction<TempFirepower>(Level);
                yield return BuffAction<LoyaltyProtectionSe>(Level * 2);
            }
        }

        private IEnumerable<BattleAction> OnHaniwaSacrificed(LoseHaniwaEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            base.NotifyActivating();
            yield return BuffAction<TempFirepower>(Level);
            yield return BuffAction<LoyaltyProtectionSe>(Level * 2);
        }
    }
}
