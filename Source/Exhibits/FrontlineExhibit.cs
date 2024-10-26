using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.BattleActions;
using LBoLMod.PlayerUnits;
using LBoLMod.StatusEffects.Keywords;
using System.Collections.Generic;

namespace LBoLMod.Exhibits
{
    public sealed class FrontlineExhibitDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FrontlineExhibit);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.exhibitBatchLoc.AddEntity(this);
        }

        public override ExhibitSprites LoadSprite()
        {
            return new ExhibitSprites(ResourceLoader.LoadSprite("frontlineexhibit.png", BepinexPlugin.embeddedSource));
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
                Value1: 3,
                Value2: null,
                Value3: 12,
                Mana: new ManaGroup() { White = 1 },
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.White,
                BaseManaAmount: 1,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { nameof(Haniwa) },
                RelativeCards: new List<string>() { }
            );

        }
    }

    [EntityLogic(typeof(FrontlineExhibitDef))]
    public sealed class FrontlineExhibit: ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
            base.Battle.MaxHand = Value3;
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStarting));
        }

        private IEnumerable<BattleAction> onPlayerTurnStarting(UnitEventArgs args)
        {
            var player = base.Battle.Player;
            if (player.TurnCounter == 1)
            {
                base.NotifyActivating();
                yield return new GainHaniwaAction(Value1, Value1, Value1);
            }
        }
    }
}
