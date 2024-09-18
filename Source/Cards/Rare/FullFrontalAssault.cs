using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects;
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
            cardConfig.UpgradedDamage = 10;
            cardConfig.Value1 = 7;
            cardConfig.UpgradedValue1 = 10;
            cardConfig.Keywords = Keyword.Exile | Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FullFrontalAssaultDef))]
    public sealed class FullFrontalAssault : Card
    {
        public override bool CanUse
        {
            get
            {
                var player = base.Battle.Player;
                return player.HasStatusEffect<ArcherHaniwa>() || player.HasStatusEffect<CavalryHaniwa>() || player.HasStatusEffect<FencerHaniwa>();
            }
        }
        public int TotalHaniwaDamage
        {
            get
            {
                return HaniwaUtils.TotalStanceLevel(base.Battle.Player) * Value1;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector, Damage.IncreaseBy(TotalHaniwaDamage));
        }
    }
}
