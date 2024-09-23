using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects;
using System.Collections.Generic;
using LBoLMod.Utils;

namespace LBoLMod.Cards
{
    public sealed class HealFirepowerDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HealFirepower);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Value2 = 1;
            cardConfig.UpgradedValue2 = 2;
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Firepower) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Firepower) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HealFirepowerDef))]
    public sealed class HealFirepower : Card
    {
        public int FullHeal
        {
            get
            {
                return this.Value1 + 3;
            }
        }
        public int FullFirepower
        {
            get
            {
                return this.Value2 + 1;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            var player = base.Battle.Player;
            if (HaniwaUtils.IsHaniwaFulfilled<FencerHaniwa>(player))
            {
                yield return new HealAction(player, player, FullHeal);
                yield return HaniwaUtils.RemoveDexterityIfNeeded<FencerHaniwa>(player);
            }
            else
            {
                yield return new HealAction(player, player, Value1);
            }

            if (HaniwaUtils.IsHaniwaFulfilled<ArcherHaniwa>(player))
            {
                yield return new ApplyStatusEffectAction<Firepower>(player, FullFirepower);
                yield return HaniwaUtils.RemoveDexterityIfNeeded<ArcherHaniwa>(player);
            }
            else
            {
                yield return new ApplyStatusEffectAction<Firepower>(player, Value2);
            }
        }
    }
}
