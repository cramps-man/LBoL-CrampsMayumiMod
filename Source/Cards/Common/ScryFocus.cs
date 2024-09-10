using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class ScryFocusDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ScryFocus);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Green };
            cardConfig.Cost = new ManaGroup() { Any = 1, Green = 1 };
            cardConfig.Value1 = 3;
            cardConfig.UpgradedValue1 = 4;
            cardConfig.Scry = 3;
            cardConfig.UpgradedScry = 5;
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ScryFocusDef))]
    public sealed class ScryFocus : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (StanceUtils.isStanceFulfilled<FocusStance>(base.Battle.Player))
            {
                yield return new ScryAction(Scry);
                yield return StanceUtils.RemoveDexterityIfNeeded<FocusStance>(base.Battle.Player);
            }
            yield return new DrawManyCardAction(Value1);
        }
    }
}
