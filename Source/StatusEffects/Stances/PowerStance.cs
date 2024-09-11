using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Source.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public sealed class PowerStanceDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PowerStance);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            statusConfig.RelativeEffects = new List<string>() { nameof(Stance) };
            return statusConfig;
        }
    }

    [EntityLogic(typeof(PowerStanceDef))]
    public sealed class PowerStance: ModStanceStatusEffect
    {
        public int EffectDamage
        {
            get 
            {
                return 2 + Level * 2;
            }
        }
        protected override void OnAdding(Unit unit)
        {
            base.OnAdding(unit);
            this.ReactOwnerEvent<CardUsingEventArgs>(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd && args.Card.CardType == LBoL.Base.CardType.Attack)
            {
                base.NotifyActivating();
                yield return new DamageAction(base.Battle.Player, base.Battle.RandomAliveEnemy, DamageInfo.Reaction(EffectDamage));
            }
            yield break;
        }
    }
}
