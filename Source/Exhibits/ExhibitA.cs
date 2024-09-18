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
using System.Linq;

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
                Mana: new ManaGroup() { Red = 1 },
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.Red,
                BaseManaAmount: 1,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { nameof(Haniwa) },
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
            //base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStarted));
        }

        private IEnumerable<BattleAction> onPlayerTurnStarting(UnitEventArgs args)
        {
            var player = base.Battle.Player;
            if (player.TurnCounter == 1)
            {
                base.NotifyActivating();
                yield return HaniwaUtils.ForceApplyStance<FencerHaniwa>(player, 3);
                yield return HaniwaUtils.ForceApplyStance<ArcherHaniwa>(player, 3);
                yield return HaniwaUtils.ForceApplyStance<CavalryHaniwa>(player, 3);
            }
        }

        private IEnumerable<BattleAction> onPlayerTurnStarted(UnitEventArgs args)
        {
            var player = base.Battle.Player;
            /*if (player.HasStatusEffect<PowerStance>())
            {
                base.NotifyActivating();
                var se = player.GetStatusEffect<PowerStance>();
                yield return new ApplyStatusEffectAction<TempFirepower>(player, 1 + se.Level);
            }
            if (player.HasStatusEffect<FocusStance>())
            {
                base.NotifyActivating();
                var se = player.GetStatusEffect<FocusStance>();
                yield return new DrawManyCardAction(1 + se.Level);
            }
            if (player.HasStatusEffect<CalmStance>())
            {
                base.NotifyActivating();
                var se = player.GetStatusEffect<CalmStance>();
                yield return new GainManaAction(new ManaGroup { Red = se.Level });
            }*/
            /*yield return StanceUtils.TickdownStance<PowerStance>(player);
            yield return StanceUtils.TickdownStance<FocusStance>(player);
            yield return StanceUtils.TickdownStance<CalmStance>(player);*/
            var stances = HaniwaUtils.GetAllStances(player);
            var maxLevel = stances.Max(s => s.Level);
            foreach (var item in stances.Where(s => s.Level == maxLevel))
            {
                yield return item.TickdownOrRemove();
            }
        }
    }
}
