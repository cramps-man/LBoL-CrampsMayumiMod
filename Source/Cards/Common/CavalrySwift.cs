using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CavalrySwiftDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CavalrySwift);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.Value1 = 2;
            cardConfig.Value2 = 1;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Graze) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Graze) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalrySwiftDef))]
    public sealed class CavalrySwift : Card
    {
        public int CavalryRequired => 1;
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (HaniwaUtils.IsLevelFulfilled<CavalryHaniwa>(base.Battle.Player, CavalryRequired, HaniwaActionType.Require))
            {
                yield return new ApplyStatusEffectAction<Graze>(base.Battle.Player, Value2);
            }
            yield return new DrawManyCardAction(Value1);
        }
    }
}
