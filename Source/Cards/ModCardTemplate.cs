using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.PlayerUnits;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public abstract class ModCardTemplate : CardTemplate
    {
        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(BepinexPlugin.embeddedSource);
            //imgs.AutoLoad(this, extension: ".png");
            imgs.AutoLoad("carddefault", ".jpg", "");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.cardBatchLoc.AddEntity(this);
        }

        public override CardConfig MakeConfig()
        {
            return new CardConfig(
               Index: 0,
               Id: "",
               Order: 10,
               AutoPerform: true,
               Perform: new string[0][],
               GunName: "",
               GunNameBurst: "",
               DebugLevel: 0,
               Revealable: false,

               IsPooled: true,
               FindInBattle: true,

               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Common,
               Type: CardType.Unknown,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { },
               IsXCost: false,
               Cost: new ManaGroup() { },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: null,
               UpgradedValue1: null,
               Value2: null,
               UpgradedValue2: null,
               Mana: null,
               UpgradedMana: null,
               Scry: null,
               UpgradedScry: null,

               ToolPlayableTimes: null,

               Loyalty: null,
               UpgradedLoyalty: null,
               PassiveCost: null,
               UpgradedPassiveCost: null,
               ActiveCost: null,
               UpgradedActiveCost: null,
               ActiveCost2: null,
               UpgradedActiveCost2: null,
               UltimateCost: null,
               UpgradedUltimateCost: null,

               Keywords: Keyword.None,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },

               Owner: new MayumiPlayerDef().UniqueId,
               ImageId: "",
               UpgradeImageId: "",

               Unfinished: false,
               Illustrator: null,
               SubIllustrator: new List<string>() { }
            );
        }
    }
}
