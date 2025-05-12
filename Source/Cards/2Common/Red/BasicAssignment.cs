using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class BasicAssignmentDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BasicAssignment);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 0 };
            cardConfig.Value1 = 1;
            cardConfig.Keywords = Keyword.Exile;
            cardConfig.UpgradedKeywords = Keyword.Exile;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentBonusSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign), nameof(AssignmentBonusSe) };
            cardConfig.RelativeCards = new List<string>() { nameof(ArcherPrepVolley), nameof(FencerBuildBarricade), nameof(CavalryScout) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(ArcherPrepVolley), nameof(FencerBuildBarricade), nameof(CavalryScout) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(BasicAssignmentDef))]
    public sealed class BasicAssignment : ModMayumiCard
    {
        private static bool DoneFirstAutoplay = false;
        protected override void OnEnterBattle(BattleController battle)
        {
            DoneFirstAutoplay = false;
            base.ReactBattleEvent(base.Battle.BattleStarted, this.OnBattleStarted);
        }

        private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
        {
            if (IsUpgraded && !DoneFirstAutoplay)
            {
                DoneFirstAutoplay = true;
                yield return new PlayCardAction(this);
            }
        }
        public override Interaction Precondition()
        {
            var list = new List<Card>() { Library.CreateCard<ArcherPrepVolley>(), Library.CreateCard<FencerBuildBarricade>(), Library.CreateCard<CavalryScout>() };
            return new SelectCardInteraction(1, 1, list)
            {
                Description = InteractionTitle
            };
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction selectInteraction))
                yield break;

            yield return new AddCardsToHandAction(selectInteraction.SelectedCards);
            yield return BuffAction<AssignmentBonusSe>(Value1);
        }
    }
}
