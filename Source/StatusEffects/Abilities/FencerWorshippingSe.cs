using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class FencerWorshippingSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FencerWorshippingSe);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(FencerWorshippingSeDef))]
    public sealed class FencerWorshippingSe: StatusEffect
    {
        public int Threshold => 10;
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.GainedHaniwa, this.OnGainedHaniwa);
        }

        private IEnumerable<BattleAction> OnGainedHaniwa(GainHaniwaEventArgs args)
        {
            if (args.FencerToGain <= 0)
                yield break;

            Count += args.FencerToGain;
            int timesToActivate = Count / Threshold;
            if (Count >= Threshold)
                Count -= Threshold * timesToActivate;

            if (timesToActivate > 0)
            {
                base.NotifyActivating();
                yield return new DamageAction(Owner, base.Battle.RandomAliveEnemy, DamageInfo.Reaction(timesToActivate * Level));
            }
        }
    }
}
