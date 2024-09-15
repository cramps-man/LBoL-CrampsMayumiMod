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
    public sealed class CreateHaniwaArcherDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CreateHaniwaArcher);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(ArcherHaniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(ArcherHaniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateHaniwaArcherDef))]
    public sealed class CreateHaniwaArcher:OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            return HaniwaUtils.ApplyStance<ArcherHaniwa>(base.Battle.Player, Value1);
        }
    }
}
