using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Cards;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignPlayGainBlockDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignPlayGainBlockSe);
        }
    }

    [EntityLogic(typeof(AssignPlayGainBlockDef))]
    public sealed class AssignPlayGainBlockSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            ReactOwnerEvent(Battle.CardUsed, this.OnCardUsed);
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (args.Card is ModAssignCard)
                yield return new CastBlockShieldAction(base.Battle.Player, Level, 0);
        }
    }
}
