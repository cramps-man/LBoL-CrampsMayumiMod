using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class CavalryWorshippingSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CavalryWorshippingSe);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(CavalryWorshippingSeDef))]
    public sealed class CavalryWorshippingSe: StatusEffect
    {
        public int Threshold => 10;
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(ModGameEvents.GainedHaniwa, this.OnGainedHaniwa);
        }

        private IEnumerable<BattleAction> OnGainedHaniwa(GainHaniwaEventArgs args)
        {
            if (args.CavalryToGain <= 0)
                yield break;

            Count += args.CavalryToGain;
            int timesToActivate = Count / Threshold;
            if (Count >= Threshold)
                Count -= Threshold * timesToActivate;

            if (timesToActivate > 0)
            {
                base.NotifyActivating();
                yield return BuffAction<Graze>(timesToActivate * Level);
            }
        }
    }
}
