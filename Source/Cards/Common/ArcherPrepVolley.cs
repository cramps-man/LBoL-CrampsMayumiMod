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
    public sealed class ArcherPrepVolleyDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ArcherPrepVolley);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.Nobody;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Any = 0 };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 4;
            cardConfig.Value2 = 5;
            cardConfig.UpgradedValue2 = 4;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ArcherPrepVolleyDef))]
    public sealed class ArcherPrepVolley : Card
    {
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled<ArcherHaniwa>(base.Battle.Player, ArcherRequired);
        public override string CantUseMessage => "Need more Archer";
        public int ArcherRequired => 2;
        public int StatusDamage => 5;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return HaniwaUtils.SacrificeHaniwa<ArcherHaniwa>(base.Battle.Player, ArcherRequired);
            yield return new ApplyStatusEffectAction<AssignArcherPrepVolley>(base.Battle.Player, Value1, ArcherRequired, Value2);
        }
    }
}
