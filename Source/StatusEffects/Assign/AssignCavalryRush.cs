using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Cards;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignCavalryRushDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCavalryRush);
        }
    }

    [EntityLogic(typeof(AssignCavalryRushDef))]
    public sealed class AssignCavalryRush : ModAssignStatusEffect
    {
        public DamageInfo TotalDamage => CardDamage.MultiplyBy(Level);
        public EnemyUnit Target { get; set; }

        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
        }

        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            EnemyUnit target = base.Battle.LowestHpEnemy;
            yield return new DamageAction(Owner, target, TotalDamage);
            Target = target;
            yield return PerformAction.Wait(0.5f);
        }

        protected override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            if (base.Battle.BattleShouldEnd || Target == null || Target.IsDead)
                yield break;
            Card randomFrontline = base.Battle.HandZone.Where(c => c is ModFrontlineCard).SampleOrDefault(base.GameRun.BattleRng);
            if (randomFrontline == null)
                yield break;
            foreach (var battleAction in HaniwaFrontlineUtils.ExecuteOnPlayActions(new List<Card>() { randomFrontline }, Battle, new UnitSelector(Target)))
            {
                yield return battleAction;
            };
        }
    }
}
