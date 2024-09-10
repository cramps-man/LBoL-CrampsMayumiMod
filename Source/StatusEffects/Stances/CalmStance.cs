using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects
{
    public sealed class CalmStanceDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CalmStance);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusConfig = base.MakeConfig();
            return statusConfig;
        }
    }

    public sealed class CalmStance: ModStanceStatusEffect
    {
        protected override void OnAdding(Unit unit)
        {
            base.OnAdding(unit);
            this.ReactOwnerEvent<ManaEventArgs>(Battle.ManaGained, new EventSequencedReactor<ManaEventArgs>(this.OnManaGained));
        }

        private IEnumerable<BattleAction> OnManaGained(ManaEventArgs args)
        {
            if (args.Cause == ActionCause.TurnStart)
                yield break;
            base.NotifyActivating();
            yield return new CastBlockShieldAction(base.Battle.Player, (1 + Level) * args.Value.Total, 0);
            //keep this refund mana code for another status effect
            /*var list = args.ConsumingMana.EnumerateComponents().ToList();
            var randomManaSpent = list[base.Battle.GameRun.BattleRng.NextInt(0, list.Count - 1)];
            yield return new GainManaAction(ManaGroup.Single(randomManaSpent));*/
        }
    }
}
