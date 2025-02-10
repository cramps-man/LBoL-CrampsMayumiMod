using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects.Localization;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class FullFrontalAssaultDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FullFrontalAssault);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1, Red = 1, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2, Hybrid = 1, HybridColor = 2 };
            cardConfig.Damage = 10;
            cardConfig.Value1 = 7;
            cardConfig.UpgradedValue1 = 10;
            cardConfig.Value2 = 3;
            cardConfig.Keywords = Keyword.Exile | Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FullFrontalAssaultDef))]
    public sealed class FullFrontalAssault : Card
    {
        public override bool CanUse => HaniwaUtils.HasAnyHaniwa(base.Battle.Player);
        public override string CantUseMessage => LocSe.RequiresHaniwa();
        public override int AdditionalDamage
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                return HaniwaUtils.TotalHaniwaLevel(base.Battle.Player) * Value1;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            var player = base.Battle.Player;
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, HaniwaUtils.GetHaniwaLevel<FencerHaniwa>(player), HaniwaUtils.GetHaniwaLevel<ArcherHaniwa>(player), HaniwaUtils.GetHaniwaLevel<CavalryHaniwa>(player));
        }
    }
}
