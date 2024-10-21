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
    public sealed class ExhibitADef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ExhibitA);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.exhibitBatchLoc.AddEntity(this);
        }

        public override ExhibitSprites LoadSprite()
        {
            return new ExhibitSprites(ResourceLoader.LoadSprite("assignexhibit.png", BepinexPlugin.embeddedSource));
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
                Value2: 1,
                Value3: null,
                Mana: new ManaGroup() { White = 1 },
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.White,
                BaseManaAmount: 1,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { nameof(Haniwa), nameof(Assign) },
                RelativeCards: new List<string>() { }
            );

        }
    }

    [EntityLogic(typeof(ExhibitADef))]
    public sealed class ExhibitA : ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
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
