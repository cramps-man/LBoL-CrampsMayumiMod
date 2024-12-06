using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignFencerPrepCounterDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignFencerPrepCounter);
        }
    }

    [EntityLogic(typeof(AssignFencerPrepCounterDef))]
    public sealed class AssignFencerPrepCounter : ModAssignStatusEffect
    {
        List<Unit> enemiesThatAttackedPlayer = new List<Unit>();
        public int enemiesThatAttackedPlayerCount => enemiesThatAttackedPlayer.Count;

        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
            base.ReactOwnerEvent(Owner.DamageReceiving, this.OnDamageReceiving);
            base.HandleOwnerEvent(Owner.DamageReceived, this.OnDamageReceived);
        }
        private void OnDamageReceived(DamageEventArgs args)
        {
            if (!enemiesThatAttackedPlayer.Contains(args.Source))
                enemiesThatAttackedPlayer.Add(args.Source);
            Level++;
        }

        private IEnumerable<BattleAction> OnDamageReceiving(DamageEventArgs args)
        {
            if (args.Cause == ActionCause.OnlyCalculate)
                yield break;
            yield return new CastBlockShieldAction(Owner, new BlockInfo(CardBlock));
        }

        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield break;
        }

        protected override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart, int triggerCount)
        {
            yield return new DamageAction(Owner, enemiesThatAttackedPlayer, CardDamage.MultiplyBy(Level));
        }
    }
}
