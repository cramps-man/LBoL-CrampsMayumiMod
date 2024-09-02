using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.PlayerUnits;
using LBoLMod.Source.StatusEffects;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class StanceChangeDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StanceChange);
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
               AutoPerform: false,
               Perform: new string[0][],
               GunName: "simple1",
               GunNameBurst: "simple2",
               DebugLevel: 0,
               Revealable: false,

               IsPooled: true,
               FindInBattle: true,

               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Common,
               Type: CardType.Skill,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 1 },
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
               UltimateCost: null,
               UpgradedUltimateCost: null,

               Keywords: Keyword.None,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { nameof(StanceChangePower), nameof(StanceChangeFocus), nameof(StanceChangeCalm) },
               UpgradedRelativeCards: new List<string>() { nameof(StanceChangePower), nameof(StanceChangeFocus), nameof(StanceChangeCalm) },

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

    [EntityLogic(typeof(StanceChangeDef))]
    public sealed class StanceChange:Card
    {
        public override Interaction Precondition()
        {
            var selectList = new List<Card> {};
            if (!base.Battle.Player.HasStatusEffect<PowerStance>())
            {
                selectList.Add(Library.CreateCard<StanceChangePower>());
            }
            if (!base.Battle.Player.HasStatusEffect<FocusStance>())
            {
                selectList.Add(Library.CreateCard<StanceChangeFocus>());
            }
            if (!base.Battle.Player.HasStatusEffect<CalmStance>())
            {
                selectList.Add(Library.CreateCard<StanceChangeCalm>());
            }
            return new SelectCardInteraction(1, 1, selectList);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (!(precondition is SelectCardInteraction interaction))
            {
                yield break;
            }

            foreach (Card card in interaction.SelectedCards)
            {
                OptionCard optionCard = card as OptionCard;
                if (optionCard != null)
                {
                    optionCard.SetBattle(base.Battle);
                    foreach (BattleAction battleAction in optionCard.TakeEffectActions())
                    {
                        yield return battleAction;
                    }
                }
            }
        }
    }
}
