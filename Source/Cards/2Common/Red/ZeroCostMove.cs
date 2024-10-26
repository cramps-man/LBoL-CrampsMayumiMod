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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 2;
            cardConfig.Cost = new ManaGroup() { Red = 1 };
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
                return base.Battle.DrawZone.Concat(base.Battle.DiscardZone).Where(c => c.Cost.IsEmpty).Count();
            }
        }
        public override Interaction Precondition()
        {
            if (IsUpgraded)
            {
                List<Card> cards = base.Battle.DrawZone.Concat(base.Battle.DiscardZone).Where(c => c.Cost.IsEmpty).ToList();
                return new SelectCardInteraction(0, 2, cards);
            }

            return null;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (IsUpgraded && precondition != null)
            {
                foreach (var card in ((SelectCardInteraction)precondition).SelectedCards)
                {
                    yield return new MoveCardAction(card, CardZone.Hand);
                }
            }
            else
            {
                for (int i = 0; i < Value1; i++)
                {
                    Card card = base.Battle.DrawZone.Concat(base.Battle.DiscardZone).Where(c => c.Cost.IsEmpty).SampleOrDefault(base.BattleRng);
                    if (card != null)
                        yield return new MoveCardAction(card, CardZone.Hand);
                }
            }
        }
    }
}
