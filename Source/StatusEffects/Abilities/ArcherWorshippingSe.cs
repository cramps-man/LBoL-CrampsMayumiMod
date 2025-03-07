using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class ArcherWorshippingSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ArcherWorshippingSe);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(ArcherWorshippingSeDef))]
    public sealed class ArcherWorshippingSe: StatusEffect
    {
        public int Threshold => 5;
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.GainedHaniwa, this.OnGainedHaniwa);
        }

        private IEnumerable<BattleAction> OnGainedHaniwa(GainHaniwaEventArgs args)
        {
            if (args.ArcherToGain <= 0)
                yield break;

            Count += args.ArcherToGain;
            int timesToActivate = Count / Threshold;
            if (Count >= Threshold)
                Count -= Threshold * timesToActivate;

            if (timesToActivate > 0)
            {
                base.NotifyActivating();
                for (int i = 0; i < timesToActivate * Level; i++)
                {
                    int rng = base.GameRun.BattleRng.NextInt(1, 5);
                    switch (rng)
                    {
                        case 1: yield return DebuffAction<Weak>(base.Battle.RandomAliveEnemy, duration: 2); break;
                        case 2: yield return DebuffAction<Vulnerable>(base.Battle.RandomAliveEnemy, duration: 2); break;
                        case 3: yield return DebuffAction<TempFirepowerNegative>(base.Battle.RandomAliveEnemy, level: 3); break;
                        case 4: yield return DebuffAction<LockedOn>(base.Battle.RandomAliveEnemy, level: 5); break;
                        case 5: yield return DebuffAction<Poison>(base.Battle.RandomAliveEnemy, level: 5); break;
                    }
                }
            }
        }
    }
}
