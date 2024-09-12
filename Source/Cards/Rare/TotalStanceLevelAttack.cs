using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Source.StatusEffects.Keywords;
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
            cardConfig.Cost = new ManaGroup() { Green = 2 };
            cardConfig.Damage = 5;
            cardConfig.UpgradedDamage = 7;
            cardConfig.Keywords = Keyword.Exile | Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Stance) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Stance) };
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
                return player.HasStatusEffect<PowerStance>() || player.HasStatusEffect<FocusStance>() || player.HasStatusEffect<CalmStance>();
            }
        }
        public int TotalStanceLevel
        {
            get
            {
                return StanceUtils.TotalStanceLevel(base.Battle.Player);
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            var player = base.Battle.Player;
            yield return AttackAction(selector, Damage.MultiplyBy(TotalStanceLevel));
        }
    }
}
