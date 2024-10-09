using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CreateCavalryDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CreateCavalry);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 5;
            cardConfig.Value2 = 3;
            cardConfig.Mana = new ManaGroup() { Red = 1, White = 1};
            cardConfig.UpgradedMana = new ManaGroup() { Philosophy = 2 };
            cardConfig.Keywords = Keyword.Scry;
            cardConfig.UpgradedKeywords = Keyword.Scry;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateCavalryDef))]
    public sealed class CreateCavalry : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainHaniwaAction(cavalryToGain: Value1);
            int cavalryCount = HaniwaUtils.GetHaniwaLevel<CavalryHaniwa>(base.Battle.Player);
            if (cavalryCount >= 5)
                yield return new ScryAction(new ScryInfo(Value2));

            int drawCount = 0;
            if (cavalryCount >= 3)
                drawCount++;
            if (cavalryCount >= 7)
                drawCount++;
            yield return new DrawManyCardAction(drawCount);

            if (cavalryCount >= 10)
                yield return new GainManaAction(Mana);
        }
    }
}
