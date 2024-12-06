using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class HealSacrificeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HealSacrifice);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Ability;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Value1 = 4;
            cardConfig.UpgradedValue1 = 6;
            cardConfig.Value2 = 2;
            cardConfig.UpgradedValue2 = 3;
            cardConfig.Cost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice), nameof(Firepower) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice), nameof(Firepower) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(HealSacrificeDef))]
    public sealed class HealSacrifice : Card
    {
        public int SacrificeAmount => 1;
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaActionType.Sacrifice, SacrificeAmount, SacrificeAmount, SacrificeAmount);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, SacrificeAmount, SacrificeAmount, SacrificeAmount);
            yield return HealAction(Value1);
            yield return BuffAction<Firepower>(Value2);
        }
    }
}
