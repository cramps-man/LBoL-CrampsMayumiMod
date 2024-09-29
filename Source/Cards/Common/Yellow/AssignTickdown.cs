using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.GameEvents;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class AssignTickdownDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignTickdown);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1 };
            cardConfig.Value1 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(AssignTickdownDef))]
    public sealed class AssignTickdown : Card
    {
        /*public override Interaction Precondition()
        {
            if (!IsUpgraded)
                return null;

            List<Card> list = new List<Card>();
            foreach (ModAssignStatusEffect s in Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect))
            {
                ModAssignOptionCard c = Library.CreateCard<ModAssignOptionCard>();
                c.CardName = s.Name;
                c.CardText = s.Description;
                c.StatusEffect = s;
                list.Add(c);
            };
            return new MiniSelectCardInteraction(list);
        }*/
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            /*if (precondition != null)
            {
                var c = ((MiniSelectCardInteraction)precondition).SelectedCard as ModAssignOptionCard;
                c.StatusEffect.TickdownFull();
            }*/
            foreach (ModAssignStatusEffect s in Battle.Player.StatusEffects.Where(s => s is ModAssignStatusEffect))
            {
                s.Tickdown(Value1);
            };
            yield break;
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent(ModGameEvents.AssignEffectTriggered, this.OnAssignTriggered);
        }

        private IEnumerable<BattleAction> OnAssignTriggered(AssignTriggerEventArgs args)
        {
            if (IsUpgraded && base.Zone == CardZone.Discard)
                yield return new MoveCardAction(this, CardZone.Hand);
        }
    }
}
