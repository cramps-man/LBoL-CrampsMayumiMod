using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Resource;
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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            cardConfig.RelativeCards = new List<string>() { CardTypeToSpawn.Name };
            return cardConfig;
        }
        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(BepinexPlugin.embeddedSource);
            imgs.AutoLoad(GetId().SId.Substring(6), extension: ".png", "");
            if (imgs.main == null)
                imgs.AutoLoad("carddefault", ".jpg", "");
            return imgs;
        }
    }
}
