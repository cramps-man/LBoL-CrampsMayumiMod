using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using System.Linq;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class FrontlineLoyaltyTaskLevelSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineLoyaltyTaskLevelSe);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasLevel = false;
            return config;
        }
    }

    [EntityLogic(typeof(FrontlineLoyaltyTaskLevelSeDef))]
    public sealed class FrontlineLoyaltyTaskLevelSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent(ModGameEvents.ConsumedLoyalty, this.OnConsumedLoyalty);
        }

        private void OnConsumedLoyalty(ConsumeLoyaltyEventArgs args)
        {
            foreach (var assignBuff in Owner.StatusEffects.Where(se => se is ModAssignStatusEffect))
            {
                assignBuff.Level += args.TotalConsumption;
            };
        }
    }
}
