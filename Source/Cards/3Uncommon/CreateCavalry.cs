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
            cardConfig.Scry = 3;
            cardConfig.UpgradedScry = 5;
            cardConfig.Keywords = Keyword.Scry;
            cardConfig.UpgradedKeywords = Keyword.Scry;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.RelativeCards = new List<string>() { nameof(CavalryRush) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(CavalryRush) + "+" };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateCavalryDef))]
    public sealed class CreateCavalry : Card
    {
        public int TotalDraw
        {
            get
            {
                int cavalryCount = HaniwaUtils.GetHaniwaLevel<CavalryHaniwa>(base.Battle.Player);
                int drawCount = 0;
                if (cavalryCount >= 3)
                    drawCount++;
                if (cavalryCount >= 7)
                    drawCount++;
                return drawCount;
            }
        }
        public string InteractionTitle => this.LocalizeProperty("InteractionTitle").RuntimeFormat(this.FormatWrapper);
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainHaniwaAction(cavalryToGain: Value1);
            int cavalryCount = HaniwaUtils.GetHaniwaLevel<CavalryHaniwa>(base.Battle.Player);
            if (cavalryCount >= 5)
                yield return new DescriptiveScryAction(Scry, InteractionTitle);

            yield return new DrawManyCardAction(TotalDraw);

            if (cavalryCount >= 10)
            {
                CavalryRush assignCard = Library.CreateCard<CavalryRush>(IsUpgraded);
                assignCard.SetBattle(base.Battle);
                foreach (var battleAction in assignCard.GetActions(selector, consumingMana, null, false, false, new List<DamageAction>()))
                {
                    yield return battleAction;
                }
            }
        }
    }
}
