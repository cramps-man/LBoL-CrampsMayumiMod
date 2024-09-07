using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public sealed class CalmStanceDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CalmStance);
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

    public sealed class CalmStance: ModStanceStatusEffect
    {
        private const int usesRequired = 3;
        protected override void OnAdding(Unit unit)
        {
            base.OnAdding(unit);
            this.React(StanceUtils.RemoveStance<FocusStance>(unit));
            this.React(StanceUtils.RemoveStance<PowerStance>(unit));
            this.ReactOwnerEvent<CardUsingEventArgs>(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            this.Count = usesRequired;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            this.Count--;
            if (this.Count == 0)
            {
                base.NotifyActivating();
                this.Count = usesRequired;
                yield return new GainManaAction(args.ConsumingMana);
            }
            yield break;
        }
    }
}
