using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CreateFencerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CreateFencer);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Value2 = 4;
            cardConfig.UpgradedValue2 = 6;
            cardConfig.Block = 8;
            cardConfig.UpgradedBlock = 10;
            cardConfig.Shield = 8;
            cardConfig.UpgradedShield = 10;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(TempElectric) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(TempElectric) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateFencerDef))]
    public sealed class CreateFencer : Card
    {
        public int NumUpgrade => IsUpgraded ? 2 : 1;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainHaniwaAction(fencerToGain: Value1);
            int fencerCount = HaniwaUtils.GetHaniwaLevel<FencerHaniwa>(base.Battle.Player);

            if (fencerCount >= 7)
                yield return DefenseAction(Block.Block, Shield.Shield);
            else if (fencerCount >= 3)
                yield return DefenseAction(Block.Block, 0);

            if (fencerCount >= 5)
                yield return UpgradeRandomHandAction(NumUpgrade);
            if (fencerCount >= 10)
                yield return BuffAction<TempElectric>(Value2);
        }
    }
}
