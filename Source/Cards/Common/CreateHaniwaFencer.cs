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
    public sealed class CreateHaniwaFencerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CreateHaniwaFencer);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(FencerHaniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(FencerHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateHaniwaFencerDef))]
    public sealed class CreateHaniwaFencer:OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            return HaniwaUtils.GainHaniwa<FencerHaniwa>(Battle.Player, Value1);
        }
    }
}
