using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Cards;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.StatusEffects.Abilities
{
    public sealed class ExileFrontlineBlitzSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ExileFrontlineBlitzSe);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var config = base.MakeConfig();
            config.HasLevel = false;
            return config;
        }
    }

    [EntityLogic(typeof(ExileFrontlineBlitzSeDef))]
    public sealed class ExileFrontlineBlitzSe: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent(base.Battle.CardExiled, this.OnCardExiled);
        }

        private IEnumerable<BattleAction> OnCardExiled(CardEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            if (args.Card is ModFrontlineCard frontlineCard && !(args.Card is HaniwaCommander))
            {
                base.NotifyActivating();
                frontlineCard.NotifyActivating();
                foreach (var action in frontlineCard.GetActions(HaniwaFrontlineUtils.GetTargetForOnPlayAction(base.Battle), new ManaGroup(), null, new List<DamageAction>(), false))
                {
                    if (base.Battle.BattleShouldEnd)
                        yield break;
                    yield return action;
                }
            }
        }
    }
}
