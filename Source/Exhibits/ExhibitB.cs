using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.PlayerUnits;
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
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );

        }
    }

    [EntityLogic(typeof(ExhibitBDef))]
    public sealed class ExhibitB: ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
            base.ReactBattleEvent<StatusEffectApplyEventArgs>(base.Battle.Player.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.onStatusApplied));
        }

        private IEnumerable<BattleAction> onStatusApplied(StatusEffectApplyEventArgs args)
        {
            if (args.Effect is PowerStance)
            {
                yield return new ApplyStatusEffectAction<TempFirepower>(base.Battle.Player, 1);
            }
            if (args.Effect is FocusStance)
            {
                yield return new DrawCardAction();
            }
            if (args.Effect is CalmStance)
            {
                yield return new CastBlockShieldAction(base.Battle.Player, base.Battle.Player, 4, 0);
            }
            yield break;
        }
    }
}
