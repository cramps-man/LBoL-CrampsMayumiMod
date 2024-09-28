using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class ModAssignOptionCardDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ModAssignOptionCard);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.HideMesuem = true;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ModAssignOptionCardDef))]
    public sealed class ModAssignOptionCard: Card
    {
        public override string Name => CardName == "" ? base.Name : CardName;
        public override string Description => CardText == "" ? base.Description : CardText;
        public ModAssignStatusEffect StatusEffect { get; set; } = null;

        public string CardName { get; set; } = "";
        public string CardText { get; set; } = "";
    }
}
