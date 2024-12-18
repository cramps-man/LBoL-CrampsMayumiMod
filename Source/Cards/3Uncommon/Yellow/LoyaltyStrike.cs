using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class LoyaltyStrikeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LoyaltyStrike);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Damage = 18;
            cardConfig.Cost = new ManaGroup() { White = 2, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.Keywords = Keyword.Accuracy;
            cardConfig.UpgradedKeywords = Keyword.Accuracy;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(LoyaltyStrikeDef))]
    public sealed class LoyaltyStrike : Card
    {
        public int HaniwaBonus
        {
            get
            {
                if (base.Battle != null)
                    return base.Battle.HandZone.Sum(c => c is ModFrontlineCard m ? m.RemainingValue : 0);
                return 0;
            }
        }

        public override int AdditionalDamage => HaniwaBonus;
    }
}
