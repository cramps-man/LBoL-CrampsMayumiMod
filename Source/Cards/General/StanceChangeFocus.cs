using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.PlayerUnits;
using LBoLMod.Source.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class StanceChangeFocusDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StanceChangeFocus);
        }

        public override CardImages LoadCardImages()
        {
            return null;
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.cardBatchLoc.AddEntity(this);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
               Index: BepinexPlugin.sequenceTable.Next(typeof(CardConfig)),
               Id: "",
               Order: 10,
               AutoPerform: true,
               Perform: new string[0][],
               GunName: "simple1",
               GunNameBurst: "simple2",
               DebugLevel: 0,
               Revealable: false,

               IsPooled: false,
               FindInBattle: true,

               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Common,
               Type: CardType.Attack,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { ManaColor.Blue },
               IsXCost: false,
               Cost: new ManaGroup() { Red = 1 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: 2,
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
               UltimateCost: null,
               UpgradedUltimateCost: null,

               Keywords: Keyword.Accuracy,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },

               Owner: new PlayerDef().UniqueId,
               ImageId: "",
               UpgradeImageId: "",

               Unfinished: false,
               Illustrator: null,
               SubIllustrator: new List<string>() { }
            );
            return cardConfig;
        }
    }

    [EntityLogic(typeof(StanceChangeFocusDef))]
    public sealed class StanceChangeFocus:OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            yield return StanceApplier.StanceApplier.ApplyStance<FocusStance>(this);
            yield break;
        }
    }
}
