using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignReverseTickdownDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignReverseTickdown);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2, Any = 1 };
            cardConfig.Block = 10;
            cardConfig.UpgradedBlock = 15;
            cardConfig.Value1 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(TurnStartDontLoseBlock) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignReverseTickdownDef))]
    public sealed class AssignReverseTickdown : ModMayumiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return BuffAction<AssignReverseTickdownSe>(Value1);
            if (IsUpgraded)
                yield return BuffAction<TurnStartDontLoseBlock>(1);
        }
    }
}
