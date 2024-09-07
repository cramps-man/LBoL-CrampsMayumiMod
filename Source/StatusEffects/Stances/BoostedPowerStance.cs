using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public sealed class BoostedPowerStanceDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BoostedPowerStance);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.IsStackable = false;
            statusConfig.HasLevel = false;
            statusConfig.HasDuration = true;
            statusConfig.DurationDecreaseTiming = LBoL.Base.DurationDecreaseTiming.TurnStart;
            return statusConfig;
        }
    }

    public sealed class BoostedPowerStance: StatusEffect
    {
        protected override void OnAdding(Unit unit)
        {
            this.ReactOwnerEvent<CardUsingEventArgs>(Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
        }

        private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
        {
            base.NotifyActivating();
            yield return new DamageAction(base.Battle.Player, base.Battle.RandomAliveEnemy, DamageInfo.Reaction(6));
        }
    }
}
