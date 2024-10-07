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
            return nameof(MassRessurection);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.AllEnemies;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2, Any = 1 };
            cardConfig.Damage = 20;
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.RelativeKeyword = Keyword.Exile;
            cardConfig.UpgradedRelativeKeyword = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(MassRessurectionDef))]
    public sealed class MassRessurection : Card
    {
        public override int AdditionalDamage
        {
            get
            {
                if (base.Battle != null)
                    return base.Battle.ExileZone.Where(c => c is ModFrontlineCard).Count() * Value1;
                return 0;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;

            var cards = base.Battle.ExileZone.Where(c => c is ModFrontlineCard).ToList();
            foreach (var card in cards)
            {
                yield return new MoveCardAction(card, CardZone.Discard);
            }
        }
    }
}
