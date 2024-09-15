using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.Source.StatusEffects.Stances;
using System.Collections.Generic;
/*
namespace LBoLMod.Cards
{
    public sealed class GrantDexterityDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(GrantDexterity);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Green };
            cardConfig.Block = 6;
            cardConfig.UpgradedBlock = 9;
            cardConfig.Value1 = 1;
            cardConfig.Cost = new ManaGroup() { Green = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Dexterity) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Dexterity) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(GrantDexterityDef))]
    public sealed class GrantDexterity : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction(RawBlock, 0);
            yield return BuffAction<Dexterity>(Value1);
        }
    }
}
*/