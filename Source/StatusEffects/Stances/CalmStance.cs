using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Source.StatusEffects.Keywords;
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
            statusConfig.RelativeEffects = new List<string>() { nameof(Stance) };
            return statusConfig;
        }
    }

    [EntityLogic(typeof(CalmStanceDef))]
    public sealed class CalmStance: ModStanceStatusEffect
    {
        public int EffectValue
        {
            get
            {
                return 1 + Level;
            }
        }
        protected override void OnAdding(Unit unit)
        {
            base.OnAdding(unit);
            //this.ReactOwnerEvent<ManaEventArgs>(Battle.ManaGained, new EventSequencedReactor<ManaEventArgs>(this.OnManaGained));
        }

        private IEnumerable<BattleAction> OnManaGained(ManaEventArgs args)
        {
            if (args.Cause == ActionCause.TurnStart)
                yield break;
            base.NotifyActivating();
            yield return new CastBlockShieldAction(base.Battle.Player, EffectValue * args.Value.Total, 0);
            //keep this refund mana code for another status effect
            /*var list = args.ConsumingMana.EnumerateComponents().ToList();
            var randomManaSpent = list[base.Battle.GameRun.BattleRng.NextInt(0, list.Count - 1)];
            yield return new GainManaAction(ManaGroup.Single(randomManaSpent));*/
        }
    }
}
