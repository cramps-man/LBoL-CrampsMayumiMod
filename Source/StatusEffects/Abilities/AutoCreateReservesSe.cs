using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Cards;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AutoCreateReservesSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AutoCreateReservesSe);
        }
    }

    [EntityLogic(typeof(AutoCreateReservesSeDef))]
    public sealed class AutoCreateReservesSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            ReactOwnerEvent(base.Battle.Player.TurnStarted, this.OnTurnStarted);
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            int count = base.Battle.HandZone.OfType<CreateHaniwa>().Count();
            if (count >= Level)
                yield break;

            base.NotifyActivating();
            yield return new AddCardsToHandAction(Library.CreateCards<CreateHaniwa>(Level - count));
        }
    }
}
