using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignCavalrySuppliesDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCavalrySupplies);
        }
    }

    [EntityLogic(typeof(AssignCavalrySuppliesDef))]
    public sealed class AssignCavalrySupplies : ModAssignStatusEffect
    {
        public ManaGroup RedMana => ManaGroup.Reds(CardValue2);
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield return new GainManaAction(RedMana);
        }

        protected override IEnumerable<BattleAction> AfterAssignmentDone(bool onTurnStart)
        {
            yield return new GainManaAction(CardMana);
        }
    }
}
