using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Linq;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class ZeroCostReductionSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ZeroCostReductionSe);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(ZeroCostReductionSeDef))]
    public sealed class ZeroCostReductionSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent(base.Battle.CardUsed, this.OnCardUsed);
            base.HandleOwnerEvent(base.Battle.Player.TurnStarted, this.OnTurnStarted);
        }

        public override bool Stack(StatusEffect other)
        {
            base.Stack(other);
            Count += other.Level;
            return true;
        }

        private void OnTurnStarted(UnitEventArgs args)
        {
            Count = Level;
        }

        private void OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                return;
            if (Count <= 0)
                return;

            if (args.ConsumingMana.IsEmpty)
            {
                Card card = base.Battle.HandZone.Where(c => c.Cost.Any > 0 && !c.IsForbidden).SampleOrDefault(base.GameRun.BattleRng);
                if (card != null)
                {
                    base.NotifyActivating();
                    card.DecreaseTurnCost(ManaGroup.Anys(1));
                    Count--;
                }
            }
        }
    }
}
