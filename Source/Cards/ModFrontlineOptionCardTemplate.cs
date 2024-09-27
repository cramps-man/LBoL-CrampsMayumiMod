using LBoL.Base;
using LBoL.ConfigData;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public abstract class ModFrontlineOptionCardTemplate : ModCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.HideMesuem = true;
            cardConfig.Type = CardType.Skill;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            return cardConfig;
        }
    }
}
