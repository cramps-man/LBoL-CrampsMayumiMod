using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Cards;
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
        public ModFrontlineCard RandomFrontline { get; set; }
        public Interaction Precondition { get; set; }

        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
        }

        protected override IEnumerable<BattleAction> BeforeAssignmentDone(bool onTurnStart)
        {
            EnemyUnit target = base.Battle.LowestHpEnemy;
            yield return new DamageAction(Owner, target, TotalDamage);
            Target = target;
        }

        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            if (base.Battle.BattleShouldEnd || Target == null || Target.IsDead)
                yield break;
            RandomFrontline = base.Battle.HandZone.Where(c => c is ModFrontlineCard).SampleOrDefault(base.GameRun.BattleRng) as ModFrontlineCard;
            if (RandomFrontline == null)
                yield break;
            Precondition = RandomFrontline.Precondition();
            if (Precondition != null)
            {
                Precondition.Description = RandomFrontline.ExtraDescription1.RuntimeFormat(RandomFrontline.FormatWrapper);
                yield return new InteractionAction(Precondition, true);
            }
            else
                yield return PerformAction.Wait(0.5f);
        }

        protected override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            if (RandomFrontline == null)
                yield break;
            RandomFrontline.NotifyActivating();
            foreach (var action in RandomFrontline.GetActions(new UnitSelector(Target), ManaGroup.Empty, Precondition, new List<DamageAction>(), false))
            {
                if (base.Battle.BattleShouldEnd)
                    yield break;
                yield return action;
            }
        }
    }
}
