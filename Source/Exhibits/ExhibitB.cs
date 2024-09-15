using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.Cards;
using LBoLMod.PlayerUnits;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects;
using System.Collections.Generic;

namespace LBoLMod.Exhibits
{
    public sealed class ExhibitBDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ExhibitB);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.exhibitBatchLoc.AddEntity(this);
        }

        public override ExhibitSprites LoadSprite()
        {
            return new ExhibitSprites();
        }

        public override ExhibitConfig MakeConfig()
        {
            return new ExhibitConfig(
                Index: 0,
                Id: "",
                Order: 10,
                IsDebug: false,
                IsPooled: false,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.Nowhere,
                Owner: new PlayerDef().UniqueId,
                LosableType: ExhibitLosableType.DebutLosable,
                Rarity: Rarity.Shining,
                Value1: null,
                Value2: null,
                Value3: null,
                Mana: new ManaGroup() { Green = 1 },
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.Green,
                BaseManaAmount: 1,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { nameof(Haniwa) },
                RelativeCards: new List<string>() { }
            );

        }
    }

    [EntityLogic(typeof(ExhibitBDef))]
    public sealed class ExhibitB: ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
            //base.ReactBattleEvent<StatusEffectApplyEventArgs>(base.Battle.Player.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.onStatusApplied));
            //base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStarting));
            //base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStarted));
        }

        private IEnumerable<BattleAction> onPlayerTurnStarted(UnitEventArgs args)
        {
            var player = base.Battle.Player;
            foreach (var item in HaniwaUtils.GetAllStances(player))
            {
                yield return item.TickdownOrRemove();
            };
        }

        private IEnumerable<BattleAction> onPlayerTurnStarting(UnitEventArgs args)
        {
            var stanceChangeCount = 0;
            foreach(Card card in base.Battle.HandZone)
            {
                if (card is CreateHaniwa)
                {
                    stanceChangeCount++;
                }
            }
            if (stanceChangeCount < 1)
            {
                base.NotifyActivating();
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<CreateHaniwa>() });
            }
        }

        private IEnumerable<BattleAction> onStatusApplied(StatusEffectApplyEventArgs args)
        {
            if (args.Effect is ArcherHaniwa)
            {
                base.NotifyActivating();
                yield return new ApplyStatusEffectAction<TempFirepower>(base.Battle.Player, 1);
            }
            if (args.Effect is CavalryHaniwa)
            {
                base.NotifyActivating();
                yield return new DrawCardAction();
            }
            if (args.Effect is FencerHaniwa)
            {
                base.NotifyActivating();
                yield return new GainManaAction(ManaGroup.Greens(1));
            }
        }
    }
}
