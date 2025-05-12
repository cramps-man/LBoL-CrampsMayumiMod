using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
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
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Value2 = 5;
            cardConfig.Block = 10;
            cardConfig.Shield = 10;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Reflect), nameof(TempElectric) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Reflect), nameof(TempElectric) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateFencerDef))]
    public sealed class CreateFencer : ModMayumiCard
    {
        public int NumUpgrade => 2;
        public int ReflectionGain => IsUpgraded ? 8 : 5;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainHaniwaAction(fencerToGain: Value1);
            int fencerCount = HaniwaUtils.GetHaniwaLevel<FencerHaniwa>(base.Battle.Player);

            if (fencerCount >= 7)
                yield return DefenseAction(Block.Block, Shield.Shield);
            else
                yield return DefenseAction(Block.Block, 0);

            if (fencerCount >= 3)
                yield return BuffAction<Reflect>(ReflectionGain);
            if (fencerCount >= 5)
                yield return UpgradeRandomHandAction(NumUpgrade);
            if (fencerCount >= 10)
                yield return BuffAction<TempElectric>(Value2);
        }
    }
}
