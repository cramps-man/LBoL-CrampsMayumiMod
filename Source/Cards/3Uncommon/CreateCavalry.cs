using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
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
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Value2 = 1;
            cardConfig.Scry = 2;
            cardConfig.UpgradedScry = 3;
            cardConfig.Mana = new ManaGroup() { Philosophy = 1 };
            cardConfig.Keywords = Keyword.Scry;
            cardConfig.UpgradedKeywords = Keyword.Scry;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Graze) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Graze) };
            cardConfig.RelativeCards = new List<string>() { nameof(CavalryRush) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(CavalryRush)};
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateCavalryDef))]
    public sealed class CreateCavalry : ModMayumiCard
    {
        public int TotalDraw
        {
            get
            {
                int cavalryCount = HaniwaUtils.GetHaniwaLevel<CavalryHaniwa>(base.Battle.Player);
                int drawCount = 2;
                return drawCount;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainHaniwaAction(cavalryToGain: Value1);
            int cavalryCount = HaniwaUtils.GetHaniwaLevel<CavalryHaniwa>(base.Battle.Player);
            if (cavalryCount >= 3)
                yield return new DescriptiveScryAction(Scry, InteractionTitle);

            yield return new DrawManyCardAction(TotalDraw);
            if (cavalryCount >= 5)
                yield return BuffAction<Graze>(Value2);
            if (cavalryCount >= 7)
                yield return new GainManaAction(Mana);
            if (cavalryCount >= 10)
            {
                CavalryRush assignCard = Library.CreateCard<CavalryRush>();
                assignCard.SetBattle(base.Battle);
                foreach (var battleAction in assignCard.GetActions(selector, consumingMana, null, false, false, new List<DamageAction>()))
                {
                    yield return battleAction;
                }
            }
        }
    }
}
