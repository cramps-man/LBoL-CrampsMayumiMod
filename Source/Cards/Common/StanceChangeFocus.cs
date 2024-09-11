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
    public sealed class StanceChangeFocusDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StanceChangeFocus);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Green };
            cardConfig.RelativeEffects = new List<string>() { nameof(FocusStance) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(FocusStance) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StanceChangeFocusDef))]
    public sealed class StanceChangeFocus:OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            return StanceUtils.ApplyStance<FocusStance>(base.Battle.Player);
        }
    }
}
