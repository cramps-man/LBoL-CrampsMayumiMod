using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Assign;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CavalrySuppliesDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CavalrySupplies);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 0 };
            cardConfig.Value1 = 8;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalrySuppliesDef))]
    public sealed class CavalrySupplies : Card
    {
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled<CavalryHaniwa>(base.Battle.Player, CavalryRequired);
        public override string CantUseMessage => "Need more Cavalry";
        public int CavalryRequired => 2;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return HaniwaUtils.SacrificeHaniwa<CavalryHaniwa>(base.Battle.Player, CavalryRequired);
            yield return new ApplyStatusEffectAction<AssignCavalrySupplies>(base.Battle.Player, 0, CavalryRequired, Value1);
        }
    }
}
