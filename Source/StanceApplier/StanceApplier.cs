using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLMod.StatusEffects;

namespace LBoLMod.StanceApplier
{
    public static class StanceApplier
    {
        public static BattleAction ApplyPowerStance(Card card)
        {
            if (!card.Battle.Player.HasStatusEffect<PowerStance>())
                return card.BuffAction<PowerStance>();
            return null;
        }

        public static BattleAction RemovePowerStance(Card card)
        {
            return new RemoveStatusEffectAction(card.Battle.Player.GetStatusEffect<PowerStance>());
        }
    }
}
