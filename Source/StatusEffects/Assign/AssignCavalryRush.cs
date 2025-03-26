using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Cards;
using System;
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
        public int TotalFrontlineCount => Math.Max(Level / CardValue1, 1);
        public EnemyUnit Target { get; set; }
        public Dictionary<ModFrontlineCard, Interaction> RandomFrontlines { get; set; } = new Dictionary<ModFrontlineCard, Interaction>();

        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
        }

        public override IEnumerable<BattleAction> BeforeAssignmentDone(bool onTurnStart)
        {
            EnemyUnit target = base.Battle.LowestHpEnemy;
            yield return new DamageAction(Owner, target, TotalDamage);
            Target = target;
            yield return PerformAction.Wait(0.3f);
        }

        public override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            List<Card> randomFrontlines = base.Battle.HandZone.Where(c => c is ModFrontlineCard).SampleManyOrAll(TotalFrontlineCount, base.GameRun.BattleRng).ToList();
            if (!randomFrontlines.Any())
                yield break;
            foreach (ModFrontlineCard frontline in randomFrontlines)
            {
                Interaction precondition = frontline.Precondition();
                RandomFrontlines.Add(frontline, precondition);
                if (precondition != null)
                {
                    precondition.Description = frontline.ExtraDescription1.RuntimeFormat(frontline.FormatWrapper);
                    yield return new InteractionAction(precondition, true);
                    yield return PerformAction.Wait(0.3f);
                }
            }
        }

        public override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            if (!RandomFrontlines.Any())
                yield break;
            foreach (var keyValue in RandomFrontlines)
            {
                ModFrontlineCard frontline = keyValue.Key;
                frontline.NotifyActivating();
                UnitSelector selector = Target.IsDead ? new UnitSelector(base.Battle.LowestHpEnemy) : new UnitSelector(Target);
                foreach (var action in frontline.GetActions(selector, ManaGroup.Empty, keyValue.Value, false, false, new List<DamageAction>()))
                {
                    if (base.Battle.BattleShouldEnd)
                        yield break;
                    yield return action;
                }
            }
        }
    }
}
