using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CreateHaniwaCavalryDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CreateHaniwaCavalry);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(CavalryHaniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(CavalryHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateHaniwaCavalryDef))]
    public sealed class CreateHaniwaCavalry:OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            yield return new GainHaniwaAction(typeof(CavalryHaniwa), Value1);
        }
    }
}
