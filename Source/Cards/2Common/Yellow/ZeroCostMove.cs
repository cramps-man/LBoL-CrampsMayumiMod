using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class ZeroCostMoveDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ZeroCostMove);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Value1 = 2;
            cardConfig.Cost = new ManaGroup() { White = 2 };
            cardConfig.UpgradedCost = new ManaGroup() { White = 1 };
            cardConfig.Mana = new ManaGroup() { Any = 0 };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ZeroCostMoveDef))]
    public sealed class ZeroCostMove : Card
    {
        public int ZeroCostCount 
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                if (IsUpgraded)
                    return base.Battle.DrawZone.Concat(base.Battle.DiscardZone).Where(c => c.Cost.IsEmpty).Count();
                return base.Battle.DrawZone.Where(c => c.Cost.IsEmpty).Count();
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            for (int i = 0; i < Value1; i++)
            {
                Card card = null;
                if (IsUpgraded)
                    card = base.Battle.DrawZone.Concat(base.Battle.DiscardZone).Where(c => c.Cost.IsEmpty).SampleOrDefault(base.BattleRng);
                else
                    card = base.Battle.DrawZone.Where(c => c.Cost.IsEmpty).SampleOrDefault(base.BattleRng);
                if (card != null)
                    yield return new MoveCardAction(card, CardZone.Hand);
            }
        }
    }
}
