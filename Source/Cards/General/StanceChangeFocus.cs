using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.PlayerUnits;
using LBoLMod.Source.StatusEffects;
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
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StanceChangeFocusDef))]
    public sealed class StanceChangeFocus:OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            yield return StanceApplier.StanceApplier.ApplyStance<FocusStance>(this);
            yield break;
        }
    }
}
