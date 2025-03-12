using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.BattleActions;
using LBoLMod.PlayerUnits;
using LBoLMod.StatusEffects.Abilities;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Exhibits
{
    public sealed class AssignExhibitDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AssignExhibit);
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
                Owner: new MayumiPlayerDef().UniqueId,
                LosableType: ExhibitLosableType.DebutLosable,
                Rarity: Rarity.Shining,
                Value1: 3,
                Value2: 1,
                Value3: 1,
                Mana: new ManaGroup() { Red = 1 },
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.Red,
                BaseManaAmount: 1,
                HasCounter: false,
                InitialCounter: 0,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { nameof(Haniwa), nameof(Assign), nameof(AssignmentBonusSe) },
                RelativeCards: new List<string>() { }
            );

        }
    }

    [EntityLogic(typeof(AssignExhibitDef))]
    public sealed class AssignExhibit : ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
            base.ReactBattleEvent(base.Battle.Player.TurnStarting, this.onPlayerTurnStarting);
            base.HandleBattleEvent(base.Battle.Player.StatusEffectAdding, this.OnStatusEffectAdding);
        }

        private void OnStatusEffectAdding(StatusEffectApplyEventArgs args)
        {
            if (args.Effect is AssignmentBonusSe)
            {
                base.NotifyActivating();
                args.Level += Value2;
                args.Effect.Level += Value2;
            }
        }

        private IEnumerable<BattleAction> onPlayerTurnStarting(UnitEventArgs args)
        {
            var player = base.Battle.Player;
            if (player.TurnCounter == 1 && !HaniwaUtils.HasAnyHaniwa(player))
            {
                base.NotifyActivating();
                yield return new GainHaniwaAction(Value1, Value1, Value1);
                yield return new ApplyStatusEffectAction<AssignmentBonusSe>(base.Battle.Player, Value3);
            }
        }
    }
}
