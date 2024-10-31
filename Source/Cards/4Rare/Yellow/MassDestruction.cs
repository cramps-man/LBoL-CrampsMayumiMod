using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class MassRessurectionDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MassDestruction);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.AllEnemies;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 2, Any = 1 };
            cardConfig.Damage = 25;
            cardConfig.UpgradedDamage = 35;
            cardConfig.Value1 = 4;
            cardConfig.UpgradedValue1 = 6;
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(MassRessurectionDef))]
    public sealed class MassDestruction : Card
    {
        public override int AdditionalDamage
        {
            get
            {
                if (base.Battle != null)
                    return CardsToExile.Count() * Value1;
                return 0;
            }
        }

        public IEnumerable<Card> CardsToExile => base.Battle.HandZone.Concat(base.Battle.DrawZone).Concat(base.Battle.DiscardZone).Where(c => c is ModFrontlineCard);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;

            yield return new ExileManyCardAction(CardsToExile);
        }
    }
}
