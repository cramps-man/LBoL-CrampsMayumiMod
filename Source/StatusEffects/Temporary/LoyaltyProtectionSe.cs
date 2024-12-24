using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class LoyaltyProtectionSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LoyaltyProtectionSe);
        }
    }

    [EntityLogic(typeof(LoyaltyProtectionSeDef))]
    public sealed class LoyaltyProtectionSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.ConsumingLoyalty, this.OnConsumingLoyalty);
        }

        private IEnumerable<BattleAction> OnConsumingLoyalty(ConsumeLoyaltyEventArgs args)
        {
            if (args.LoyaltyConsumption <= 0)
                yield break;

            base.NotifyActivating();
            int amountReduced = 0;
            if (Level > args.LoyaltyConsumption)
            {
                Level -= args.LoyaltyConsumption;
                amountReduced += args.LoyaltyConsumption;
                args.LoyaltyConsumption = 0;
            }
            else
            {
                args.LoyaltyConsumption -= Level;
                amountReduced += Level;
                Level = 0;
            }

            yield return new CastBlockShieldAction(base.Owner, amountReduced, 0);
            if (Level <= 0)
                yield return new RemoveStatusEffectAction(this);
        }
    }
}
