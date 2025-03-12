using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class AssignPlayDrawSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignPlayDrawSe);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(AssignPlayDrawSeDef))]
    public sealed class AssignPlayDrawSe: StatusEffect
    {
        public int BuffCount => 5;
        public int RemainingCount => BuffCount - Count;
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(base.Battle.Player.StatusEffectAdded, this.OnStatusEffectAdded);
        }
        private IEnumerable<BattleAction> OnStatusEffectAdded(StatusEffectApplyEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (args.Effect.Type != StatusEffectType.Positive || args.Effect is AssignmentBonusSe)
                yield break;

            Count++;
            if (Count >= 5)
            {
                base.NotifyActivating();
                yield return BuffAction<AssignmentBonusSe>(Level);
                Count = 0;
            }
        }
    }
}
