using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Debuffs;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class AssignmentOrderMarkDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignmentOrderMark);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.HideMesuem = true;
            cardConfig.IsUpgradable = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Value1 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(AssignmentMarkSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(AssignmentMarkSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignmentOrderMarkDef))]
    public sealed class AssignmentOrderMark : OptionCard
    {
        public UnitSelector selector = null;
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            if (selector != null)
                yield return DebuffAction<AssignmentMarkSe>(selector.SelectedEnemy, duration: Value1);
        }
    }
}
