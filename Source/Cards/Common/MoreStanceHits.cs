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
    public sealed class MoreStanceHitsDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MoreStanceHits);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.RandomEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 2 };
            cardConfig.Damage = 8;
            cardConfig.UpgradedDamage = 10;
            cardConfig.RelativeEffects = new List<string>() { nameof(Stance) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Stance) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(MoreStanceHitsDef))]
    public sealed class MoreStanceHits : Card
    {
        public int StanceCount
        {
            get
            {
                var count = 0;
                var player = base.Battle.Player;
                if (player.HasStatusEffect<PowerStance>())
                    count++;
                if (player.HasStatusEffect<FocusStance>())
                    count++;
                if (player.HasStatusEffect<CalmStance>())
                    count++;
                return count;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            for (int i = 0; i < StanceCount; i++) 
            {
                yield return AttackAction(selector);
            }
        }
    }
}
