using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class CommandersMarkSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CommandersMarkSe);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.Type = StatusEffectType.Negative;
            config.HasLevel = false;
            config.IsStackable = false;
            return config;
        }
    }

    [EntityLogic(typeof(CommandersMarkSeDef))]
    public sealed class CommandersMarkSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            foreach (EnemyUnit enemy in base.Battle.AllAliveEnemies)
            {
                base.ReactOwnerEvent(enemy.StatusEffectAdded, this.OnEnemyStatusAdded);
            }
            base.HandleOwnerEvent(base.Battle.EnemySpawned, this.OnEnemySpawned);
        }

        private void OnEnemySpawned(UnitEventArgs args)
        {
            base.ReactOwnerEvent(args.Unit.StatusEffectAdded, this.OnEnemyStatusAdded);
        }

        private IEnumerable<BattleAction> OnEnemyStatusAdded(StatusEffectApplyEventArgs args)
        {
            if (args.Effect != this && args.Effect is CommandersMarkSe)
            {
                yield return new RemoveStatusEffectAction(this);
            }
        }
    }
}
