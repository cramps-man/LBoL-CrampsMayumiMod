using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignPlayGainBlockDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignPlayGainBlock);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 4;
            cardConfig.UpgradedValue1 = 6;
            cardConfig.Value2 = 1;
            cardConfig.UpgradedValue2 = 2;
            cardConfig.Cost = new ManaGroup() { Red = 2, Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignPlayGainBlockDef))]
    public sealed class AssignPlayGainBlock : ModMayumiCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<AssignPlayGainBlockSe>(level: Value1);
            yield return new AddCardsToHandAction(Library.CreateCards<AssignmentOrder>(Value2));
        }
    }
}
