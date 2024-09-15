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
    public sealed class TotalStanceLevelAttackDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(TotalStanceLevelAttack);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Green };
            cardConfig.Cost = new ManaGroup() { Green = 1, Red = 1, Any = 1 };
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

    [EntityLogic(typeof(TotalStanceLevelAttackDef))]
    public sealed class TotalStanceLevelAttack : Card
    {
        public override bool CanUse
        {
            get
            {
                var player = base.Battle.Player;
                return player.HasStatusEffect<ArcherHaniwa>() || player.HasStatusEffect<CavalryHaniwa>() || player.HasStatusEffect<FencerHaniwa>();
            }
        }
        public int TotalStanceDamage
        {
            get
            {
                return HaniwaUtils.TotalStanceLevel(base.Battle.Player) * Value1;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector, Damage.IncreaseBy(TotalStanceDamage));
        }
    }
}
