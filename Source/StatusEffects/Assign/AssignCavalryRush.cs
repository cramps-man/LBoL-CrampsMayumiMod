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
        public EnemyUnit Target { get; set; }
        public Dictionary<Card, Interaction> RandomCommandedCards { get; set; } = new Dictionary<Card, Interaction>();

        protected override void OnAdded(Unit unit)
        {
            base.OnAdded(unit);
        }

        public override IEnumerable<BattleAction> BeforeAssignmentDone(bool onTurnStart)
        {
            RandomCommandedCards.Clear();
            EnemyUnit target = base.Battle.LowestHpEnemy;
            yield return new DamageAction(Owner, target, TotalDamage);
            Target = target;
            yield return PerformAction.Wait(0.3f);
        }

        public override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            List<Card> randomCommandedCards = HaniwaFrontlineUtils.GetCommandableCards(base.Battle.HandZone.ToList()).SampleManyOrAll(TotalCommandableCount, base.GameRun.BattleRng).ToList();
            if (!randomCommandedCards.Any())
                yield break;
            foreach (Card card in randomCommandedCards)
            {
                Interaction precondition = card.Precondition();
                RandomCommandedCards.Add(card, precondition);
                if (precondition != null)
                {
                    if (card.ExtraDescription1 != null && card is ModFrontlineCard)
                        precondition.Description = card.ExtraDescription1.RuntimeFormat(card.FormatWrapper);
                    else
                        precondition.Description = card.Name;
                    yield return new InteractionAction(precondition, true);
                    yield return PerformAction.Wait(0.3f);
                }
            }
        }

        public override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            if (!RandomCommandedCards.Any())
                yield break;
            foreach (var keyValue in RandomCommandedCards)
            {
                Card card = keyValue.Key;
                card.NotifyActivating();
                UnitSelector selector = Target.IsDead ? new UnitSelector(base.Battle.LowestHpEnemy) : new UnitSelector(Target);
                foreach (var action in card.GetActions(selector, ManaGroup.Empty, keyValue.Value, false, false, new List<DamageAction>()))
                {
                    if (base.Battle.BattleShouldEnd)
                        yield break;
                    yield return action;
                }
                if (card.IsExile || card.CardType == CardType.Ability)
                    card.IsCopy = true;
            }
        }
    }
}
