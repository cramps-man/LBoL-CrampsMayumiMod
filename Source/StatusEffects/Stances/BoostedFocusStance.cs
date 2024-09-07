using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public sealed class BoostedFocusStanceDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BoostedFocusStance);
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

    public sealed class BoostedFocusStance: StatusEffect
    {
        protected override void OnAdding(Unit unit)
        {
            this.ReactOwnerEvent<CardUsingEventArgs>(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            base.NotifyActivating();
            yield return new DrawManyCardAction(1);
        }
    }
}
