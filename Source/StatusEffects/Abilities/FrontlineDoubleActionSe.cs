using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.Cards;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class FrontlineDoubleActionSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineDoubleActionSe);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasLevel = false;
            return config;
        }
    }

    [EntityLogic(typeof(FrontlineDoubleActionSeDef))]
    public sealed class FrontlineDoubleActionSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(base.Battle.CardUsed, this.OnCardUsed);
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;
            if (args.Selector.Type == TargetType.SingleEnemy && args.Selector.SelectedEnemy.IsDead)
                yield break;

            if (args.Card is ModFrontlineCard frontlineCard)
            {
                base.NotifyActivating();
                yield return new CommandAction(new List<Card> { frontlineCard }, args.Selector, true);
            }
        }
    }
}
