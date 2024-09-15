using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class BlockCalmDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BlockCalm);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Green };
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 9 };
            cardConfig.Block = 8;
            cardConfig.UpgradedBlock = 12;
            cardConfig.Shield = 8;
            cardConfig.UpgradedShield = 12;
            cardConfig.RelativeEffects = new List<string>() { nameof(FencerHaniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(FencerHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BlockCalmDef))]
    public sealed class BlockCalm : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (HaniwaUtils.isStanceFulfilled<FencerHaniwa>(base.Battle.Player))
            {
                yield return base.DefenseAction(RawBlock, RawShield);
                yield return HaniwaUtils.RemoveDexterityIfNeeded<FencerHaniwa>(base.Battle.Player);
            }
            yield return base.DefenseAction(RawBlock, 0);
        }
    }
}
