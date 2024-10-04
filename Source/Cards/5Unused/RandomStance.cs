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
/*
namespace LBoLMod.Cards
{
    public sealed class RandomStanceDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(RandomStance);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.SingleEnemy;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 14;
            cardConfig.UpgradedDamage = 19;
            cardConfig.Value1 = 1;
            cardConfig.Cost = new ManaGroup() { Red = 1, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Preserve) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Preserve) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(RandomStanceDef))]
    public sealed class RandomStance : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd)
                yield break;

            var randomPick = base.BattleRng.NextInt(1, 3);
            var player = base.Battle.Player;
            if (randomPick == 1)
            {
                foreach (var item in HaniwaUtils.ApplyStance<ArcherHaniwa>(player, Value1))
                {
                    yield return item;
                };
                HaniwaUtils.PreserveSpecifiedStance<ArcherHaniwa>(player);
            }
            else if (randomPick == 2)
            {
                foreach (var item in HaniwaUtils.ApplyStance<CavalryHaniwa>(player, Value1))
                {
                    yield return item;
                };
                HaniwaUtils.PreserveSpecifiedStance<CavalryHaniwa>(player);
            }
            else if (randomPick == 3)
            {
                foreach (var item in HaniwaUtils.ApplyStance<FencerHaniwa>(player, Value1))
                {
                    yield return item;
                };
                HaniwaUtils.PreserveSpecifiedStance<FencerHaniwa>(player);
            }
        }
    }
}*/
