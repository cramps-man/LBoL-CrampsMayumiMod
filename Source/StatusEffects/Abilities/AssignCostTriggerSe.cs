using LBoL.Base;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Cards;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignCostTriggerSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCostTriggerSe);
        }
    }

    [EntityLogic(typeof(AssignCostTriggerSeDef))]
    public sealed class AssignCostTriggerSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent(base.Battle.Player.StatusEffectAdding, this.OnStatusEffectAdding);
            foreach (var card in Battle.EnumerateAllCards())
            {
                if (card is ModAssignCard assignCard)
                {
                    assignCard.IncreaseBaseCost(ManaGroup.Anys(1));
                }
            };
        }

        private void OnStatusEffectAdding(StatusEffectApplyEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                return;
            if (args.Effect is ModAssignStatusEffect assignStatus)
            {
                assignStatus.Level += Level;
            }
        }
    }
}
