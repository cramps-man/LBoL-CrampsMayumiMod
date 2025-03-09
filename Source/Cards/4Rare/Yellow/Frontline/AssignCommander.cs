using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class AssignCommanderDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignCommander);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.Rarity = Rarity.Rare;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Value1 = 10;
            cardConfig.Value2 = 10;
            cardConfig.Keywords = Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignCommanderDef))]
    public sealed class AssignCommander : ModFrontlineCard
    {
        protected override int PassiveConsumedRemainingValue => 3;
        protected override int OnPlayConsumedRemainingValue => 0;
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault();
        public int LoyaltyGain => 3;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.HandleBattleEvent(base.Battle.Player.TurnEnded, this.OnTurnEnded);
            base.ReactBattleEvent(ModGameEvents.GainingHaniwaFromAssign, this.OnGainingHaniwaFromAssign);
        }

        private void OnTurnEnded(UnitEventArgs args)
        {
            RemainingValue += LoyaltyGain;
        }
        private IEnumerable<BattleAction> OnGainingHaniwaFromAssign(GainHaniwaEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;
            if (CheckPassiveLoyaltyNotFulfiled())
                yield break;

            base.NotifyActivating();
            yield return PerformAction.Wait(0.3f);

            int fencerCount = args.FencerToGain;
            int archerCount = args.ArcherToGain;
            int cavalryCount = args.CavalryToGain;
            Dictionary<string, int> haniwaCount = new Dictionary<string, int>()
            {
                { "fencer", fencerCount },
                { "archer", archerCount },
                { "cavalry", cavalryCount },
            };
            bool continueRandomizing = true;
            while (continueRandomizing)
            {
                var allOptions = HaniwaAssignUtils.GetAssignCardTypes(base.Battle.Player).Select(Library.CreateCard).Cast<ModAssignCard>().ToList();
                while (true)
                {
                    var randomSelection = allOptions.SampleOrDefault(base.BattleRng);
                    if (randomSelection == null)
                    {
                        continueRandomizing = false;
                        break;
                    }
                    allOptions.Remove(randomSelection);
                    if (randomSelection.FencerAssigned > haniwaCount["fencer"])
                        continue;
                    if (randomSelection.ArcherAssigned > haniwaCount["archer"])
                        continue;
                    if (randomSelection.CavalryAssigned > haniwaCount["cavalry"])
                        continue;
                    randomSelection.SetBattle(base.Battle);
                    randomSelection.ManualStack = false;
                    var assignBuffAction = randomSelection.BuffAction(randomSelection.AssignStatusType, level: randomSelection.StartingTaskLevel, count: randomSelection.StartingCardCounter);
                    yield return assignBuffAction;
                    if (assignBuffAction is ApplyStatusEffectAction sea && sea.Args.Effect is ModAssignStatusEffect mase)
                        mase.JustApplied = false;
                    haniwaCount["fencer"] -= randomSelection.FencerAssigned;
                    haniwaCount["archer"] -= randomSelection.ArcherAssigned;
                    haniwaCount["cavalry"] -= randomSelection.CavalryAssigned;
                    break;
                }
            }

            yield return ConsumePassiveLoyalty();
            base.NotifyChanged();
            args.CancelBy(this);
        }

        public override Interaction Precondition()
        {
            var interaction = new SelectCardInteraction(1, 1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player));
            interaction.Description = ExtraDescription1.RuntimeFormat(FormatWrapper);
            return interaction;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction selectInteraction))
                yield break;
            if (selectInteraction.SelectedCards == null)
                yield break;

            foreach (ModAssignOptionCard card in selectInteraction.SelectedCards)
            {
                card.StatusEffect.Level += Value2;
                var sourceCard = card.StatusEffect.AssignSourceCard;
                sourceCard.ManualStack = false;
                ApplyStatusEffectAction assignBuffAction = (ApplyStatusEffectAction)sourceCard.BuffAction(sourceCard.AssignStatusType, level: card.StatusEffect.Level, count: card.StatusEffect.Count);
                yield return assignBuffAction;
                if (assignBuffAction.Args.Effect is ModAssignStatusEffect mase)
                {
                    mase.JustApplied = false;
                    mase.IsPermanent = card.StatusEffect.IsPermanent;
                    mase.NotifyChanged();
                }
            }
        }
    }
}
