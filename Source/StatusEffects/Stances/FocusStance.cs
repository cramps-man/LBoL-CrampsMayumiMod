using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Source.StatusEffects.Keywords;
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
            statusConfig.RelativeEffects = new List<string>() { nameof(Stance) };
            return statusConfig;
        }
    }

    [EntityLogic(typeof(FocusStanceDef))]
    public sealed class FocusStance: ModStanceStatusEffect
    {
        public int EffectValue
        {
            get
            {
                return 1 + Level;
            }
        }
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
                yield return BuffAction<NextAttackUp>(EffectValue);
            }
            else if (args.Card.CardType == LBoL.Base.CardType.Defense)
            {
                yield return new CastBlockShieldAction(base.Battle.Player, EffectValue, 0);
            }
            //bit too op for base stance, maybe after ability used?
            /*else if (args.Card.CardType == LBoL.Base.CardType.Skill)
            {
                yield return new DrawManyCardAction(1);
            }*/
        }
    }
}
