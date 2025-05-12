using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class ArcherWorshippingDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ArcherWorshipping);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Black };
            cardConfig.Cost = new ManaGroup() { Black = 1, Any = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Black = 1, Any = 1 };
            cardConfig.Value1 = 1;
            cardConfig.Value2 = 5;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Weak), nameof(Vulnerable), nameof(TempFirepowerNegative), nameof(LockedOn), nameof(Poison) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Weak), nameof(Vulnerable), nameof(TempFirepowerNegative), nameof(LockedOn), nameof(Poison) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ArcherWorshippingDef))]
    public sealed class ArcherWorshipping : ModMayumiCard
    {
        public int ArcherGain => IsUpgraded ? 5 : 3;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<ArcherWorshippingSe>(Value1);
            yield return new GainHaniwaAction(archerToGain: ArcherGain);
        }
    }
}