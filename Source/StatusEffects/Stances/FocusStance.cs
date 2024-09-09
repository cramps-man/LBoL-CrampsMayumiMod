using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Basic;
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
            return statusConfig;
        }
    }

    public sealed class FocusStance: ModStanceStatusEffect
    {
        protected override void OnAdding(Unit unit)
        {
            base.OnAdding(unit);
            this.ReactOwnerEvent<CardEventArgs>(Battle.CardDrawn, new EventSequencedReactor<CardEventArgs>(this.OnCardDrawn));
        }

        private IEnumerable<BattleAction> OnCardDrawn(CardEventArgs args)
        {
            if (args.Cause == ActionCause.TurnStart)
            {
                yield break;
            }
            base.NotifyActivating();
            if (args.Card.CardType == LBoL.Base.CardType.Attack)
            {
                yield return BuffAction<NextAttackUp>(1 + Level);
            }
            else if (args.Card.CardType == LBoL.Base.CardType.Defense)
            {
                yield return new CastBlockShieldAction(base.Battle.Player, 1 + Level, 0);
            }
            else if (args.Card.CardType == LBoL.Base.CardType.Skill)
            {
                yield return new DrawManyCardAction(1);
            }
        }
    }
}
