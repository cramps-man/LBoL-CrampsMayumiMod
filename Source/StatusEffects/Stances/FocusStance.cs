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
    public sealed class FocusStanceDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FocusStance);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.IsStackable = false;
            statusConfig.HasLevel = false;
            statusConfig.HasCount = true;
            return statusConfig;
        }
    }

    public sealed class FocusStance: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            this.React(StanceApplier.StanceApplier.RemoveStance<PowerStance>(unit));
            this.React(StanceApplier.StanceApplier.RemoveStance<CalmStance>(unit));
            this.Count = 3;
            this.ReactOwnerEvent<CardUsingEventArgs>(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            this.Count--;
            if (this.Count == 0)
            {
                base.NotifyActivating();
                this.Count = 3;
                yield return new DrawManyCardAction(1);
            }
            yield break;
        }
    }
}
