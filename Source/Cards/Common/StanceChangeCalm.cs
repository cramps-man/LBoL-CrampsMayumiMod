using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class StanceChangeCalmDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StanceChangeCalm);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Blue };
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.RelativeEffects = new List<string>() { nameof(FencerHaniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(FencerHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StanceChangeCalmDef))]
    public sealed class StanceChangeCalm:OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            return HaniwaUtils.ApplyStance<FencerHaniwa>(Battle.Player, Value1);
        }
    }
}
