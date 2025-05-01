using LBoL.Base;
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
            cardConfig.Value1 = 15;
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
        protected override int PassiveConsumedRemainingValue => 7;
        protected override int OnPlayConsumedRemainingValue => 0;
        public override int AdditionalValue2 => base.UpgradeCounter.GetValueOrDefault();
        protected override void OnEnterBattle(BattleController battle)
        {
            base.OnEnterBattle(battle);
            base.ReactBattleEvent(ModGameEvents.GainingHaniwaFromAssign, this.OnGainingHaniwaFromAssign);
        }
        private IEnumerable<BattleAction> OnGainingHaniwaFromAssign(GainHaniwaEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (base.Zone != CardZone.Hand)
                yield break;
            if (CheckPassiveLoyaltyNotFulfiled())
                yield break;
            if (args.IsCanceled)
                yield break;

            base.NotifyActivating();
            yield return PerformAction.Wait(0.3f);

            foreach (var battleAction in HaniwaAssignUtils.GetRandomAssignBuffs(base.Battle, args.FencerToGain, args.ArcherToGain, args.CavalryToGain, false))
            {
                yield return battleAction;
            };

            yield return ConsumePassiveLoyalty();
            base.NotifyChanged();
            args.CancelBy(this);
        }

        public override Interaction Precondition()
        {
            var interaction = new SelectCardInteraction(1, 1, HaniwaAssignUtils.CreateAssignOptionCards(base.Battle.Player).Where(c => c.StatusEffect.Count != 0));
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
