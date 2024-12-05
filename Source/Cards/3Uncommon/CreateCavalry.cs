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
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainHaniwaAction(cavalryToGain: Value1);
            int cavalryCount = HaniwaUtils.GetHaniwaLevel<CavalryHaniwa>(base.Battle.Player);
            if (cavalryCount >= 5)
                yield return new ScryAction(Scry);

            int drawCount = 0;
            if (cavalryCount >= 3)
                drawCount++;
            if (cavalryCount >= 7)
                drawCount++;
            yield return new DrawManyCardAction(drawCount);

            if (cavalryCount >= 10)
            {
                CavalryRush assignCard = Library.CreateCard<CavalryRush>(IsUpgraded);
                yield return new LoseHaniwaAction(HaniwaActionType.Assign, assignCard.FencerAssigned, assignCard.ArcherAssigned, assignCard.CavalryAssigned);
                yield return new ApplyStatusEffectAction(assignCard.AssignStatusType, base.Battle.Player, level: 1, count: assignCard.StartingCardCounter)
                {
                    Args =
                    {
                        Effect =
                        {
                            SourceCard = assignCard
                        }
                    }
                };
            }
        }
    }
}
