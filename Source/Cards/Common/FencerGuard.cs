using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class FencerGuardDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FencerGuard);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Defense;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.Block = 8;
            cardConfig.UpgradedBlock = 12;
            cardConfig.Shield = 8;
            cardConfig.UpgradedShield = 12;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FencerGuardDef))]
    public sealed class FencerGuard : Card
    {
        public int FencerRequired => 1;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (HaniwaUtils.IsLevelFulfilled<FencerHaniwa>(base.Battle.Player, FencerRequired, HaniwaActionType.Require))
            {
                yield return base.DefenseAction(RawBlock, RawShield);
            }
            else
                yield return base.DefenseAction(RawBlock, 0);
        }
    }
}
