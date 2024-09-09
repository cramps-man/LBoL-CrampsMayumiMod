using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Exhibits;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.Cards;
using LBoLMod.PlayerUnits;
using LBoLMod.StatusEffects;
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
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );

        }
    }

    [EntityLogic(typeof(ExhibitADef))]
    public sealed class ExhibitA : ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, new EventSequencedReactor<UnitEventArgs>(this.onPlayerTurnStart));
        }

        private IEnumerable<BattleAction> onPlayerTurnStart(UnitEventArgs args)
        {
            var player = base.Battle.Player;
            if (player.HasStatusEffect<PowerStance>())
            {
                base.NotifyActivating();
                yield return new ApplyStatusEffectAction<NextAttackUp>(player, 4);
                yield return StanceUtils.ForceRemoveStance<PowerStance>(player);
            }
            if (player.HasStatusEffect<FocusStance>())
            {
                base.NotifyActivating();
                yield return new DrawManyCardAction(2);
                yield return StanceUtils.ForceRemoveStance<FocusStance>(player);
            }
            if (player.HasStatusEffect<CalmStance>())
            {
                base.NotifyActivating();
                yield return new GainManaAction(new ManaGroup { Red = 2 });
                yield return StanceUtils.ForceRemoveStance<CalmStance>(player);
            }
            if (StanceUtils.DoesPlayerHavePreservedStance(player))
            {
                yield return StanceUtils.RemoveNonPreservedStance(player);
            }
            if (player.TurnCounter == 1)
            {
                base.NotifyActivating();
                foreach (var item in StanceUtils.ApplyStance<PowerStance>(player))
                {
                    yield return item;
                }
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<StanceChange>() });
            }
            yield break;
        }
    }
}
