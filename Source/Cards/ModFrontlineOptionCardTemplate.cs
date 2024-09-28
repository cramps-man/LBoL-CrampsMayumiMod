using LBoL.Base;
using LBoL.ConfigData;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public abstract class ModFrontlineOptionCardTemplate : ModCardTemplate
    {
        protected virtual Type CardTypeToSpawn => null;
        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.HideMesuem = true;
            cardConfig.Type = CardType.Skill;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            cardConfig.RelativeCards = new List<string>() { CardTypeToSpawn.Name };
            return cardConfig;
        }
    }
}
