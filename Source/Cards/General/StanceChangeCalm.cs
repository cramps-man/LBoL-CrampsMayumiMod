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
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StanceChangeCalmDef))]
    public sealed class StanceChangeCalm:OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            yield return StanceUtils.ApplyStance<CalmStance>(this);
            yield break;
        }
    }
}
