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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 18;
            cardConfig.UpgradedDamage = 24;
            cardConfig.Value1 = 5;
            cardConfig.UpgradedValue1 = 7;
            cardConfig.Cost = new ManaGroup() { Red = 2, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Red = 1, Any = 2 };
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
                    return base.Battle.HandZone.Where(c => c is ModFrontlineCard).Count() * Value1;
                return 0;
            }
        }

        public override int AdditionalDamage => HaniwaBonus;
    }
}
