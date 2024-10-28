using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Assign
{
    public sealed class AssignCavalryScoutDef : ModAssignStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCavalryScout);
        }
    }

    [EntityLogic(typeof(AssignCavalryScoutDef))]
    public sealed class AssignCavalryScout : ModAssignStatusEffect
    {
        protected override IEnumerable<BattleAction> OnAssignmentDone(bool onTurnStart)
        {
            yield return new ScryAction(CardScry);
            yield return new DrawManyCardAction(CardValue1);
        }
    }
}
