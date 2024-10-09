using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public sealed class StrengthInNumbersDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StrengthInNumbers);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Cost = new ManaGroup() { Red = 1 };
            cardConfig.Mana = new ManaGroup() { Any = 0 };
            cardConfig.RelativeEffects = new List<string>() { nameof(TempFirepower) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(TempFirepower) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StrengthInNumbersDef))]
    public sealed class StrengthInNumbers : Card
    {
        public int ZeroCostCount
        {
            get
            {
                if (base.Battle == null)
                    return 0;
                if (IsUpgraded)
                    return base.Battle.HandZoneAndPlayArea.Concat(base.Battle.DrawZone).Concat(base.Battle.DiscardZone).Where(c => c.Cost == ManaGroup.Empty).Count();
                return base.Battle.HandZoneAndPlayArea.Where(c => c.Cost == ManaGroup.Empty).Count();
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (ZeroCostCount <= 0)
                yield break;

            yield return BuffAction<TempFirepower>(ZeroCostCount);
        }
    }
}
