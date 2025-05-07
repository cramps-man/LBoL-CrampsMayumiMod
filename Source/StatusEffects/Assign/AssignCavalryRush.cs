using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.Utils;
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
        public int TotalCommandableCount => Math.Max(Level / CardValue1, 1);

        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
        }

        public override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            var markedEnemy = MarkedEnemies.MinByOrDefault(e => e.Hp);
            EnemyUnit target = markedEnemy != null ? markedEnemy : base.Battle.LowestHpEnemy;
            yield return new DamageAction(Owner, target, TotalDamage);
            yield return PerformAction.Wait(0.3f);
            if (base.Battle.BattleShouldEnd)
                yield break;
            List<Card> randomCommandedCards = HaniwaFrontlineUtils.GetCommandableCards(base.Battle.HandZone.ToList()).SampleManyOrAll(TotalCommandableCount, base.GameRun.BattleRng).ToList();
            if (!randomCommandedCards.Any())
                yield break;
            yield return new CommandAction(randomCommandedCards, new UnitSelector(target), false, Name);
        }
    }
}
