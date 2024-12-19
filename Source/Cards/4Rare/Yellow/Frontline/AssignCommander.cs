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
using LBoLMod.StatusEffects.Abilities;
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
            cardConfig.Damage = 25;
            cardConfig.Value1 = 5;
            cardConfig.Value2 = 15;
            cardConfig.Keywords = Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Frontline), nameof(CommandersMarkSe) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Frontline), nameof(CommandersMarkSe) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignCommanderDef))]
    public sealed class AssignCommander : ModFrontlineCard
    {
        protected override int PassiveConsumedRemainingValue => 5;
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault();
        public int LoyaltyGain => 2;
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
            if (base.Zone != CardZone.Draw && base.Zone != CardZone.Hand && base.Zone != CardZone.Discard)
                yield break;
            if (RemainingValue < PassiveConsumedRemainingValue)
                yield break;

            if (base.Zone == CardZone.Hand)
            {
                base.NotifyActivating();
                yield return PerformAction.Wait(0.3f);
            }
            else if (base.Zone == CardZone.Draw || base.Zone == CardZone.Discard)
                yield return PerformAction.ViewCard(this);

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

            RemainingValue -= PassiveConsumedRemainingValue;
            base.NotifyChanged();
            args.CancelBy(this);
        }

        public override Interaction Precondition()
        {
            return new SelectCardInteraction(1, 1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player));
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction selectInteraction))
                yield break;

            foreach (ModAssignOptionCard card in selectInteraction.SelectedCards)
            {
                var sourceCard = card.StatusEffect.AssignSourceCard;
                sourceCard.ManualStack = false;
                var assignBuffAction = sourceCard.BuffAction(sourceCard.AssignStatusType, level: card.StatusEffect.Level + Value2, count: card.StatusEffect.Count);
                yield return assignBuffAction;
            }
        }
    }
}
