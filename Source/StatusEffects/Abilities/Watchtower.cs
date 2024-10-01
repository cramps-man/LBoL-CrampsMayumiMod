using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class WatchtowerDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Watchtower);
        }
    }

    [EntityLogic(typeof(WatchtowerDef))]
    public sealed class Watchtower: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(base.Battle.CardUsed, this.OnCardUsed);
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (args.Card.CardType != LBoL.Base.CardType.Attack)
                yield break;

            base.NotifyActivating();
            yield return new DamageAction(Owner, base.Battle.RandomAliveEnemy, DamageInfo.Reaction(Level));
        }
    }
}
