using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.StatusEffects
{
    public sealed class BoostedCalmStanceDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BoostedCalmStance);
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

    public sealed class BoostedCalmStance: StatusEffect
    {
        protected override void OnAdding(Unit unit)
        {
            this.ReactOwnerEvent<CardUsingEventArgs>(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            base.NotifyActivating();
            var list = args.ConsumingMana.EnumerateComponents().ToList();
            var randomManaSpent = list[base.Battle.GameRun.BattleRng.NextInt(0, list.Count - 1)];
            yield return new GainManaAction(ManaGroup.Single(randomManaSpent));
        }
    }
}
