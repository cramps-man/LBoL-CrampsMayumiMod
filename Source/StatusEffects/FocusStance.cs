using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.StatusEffects;
using System.Collections.Generic;
using UnityEngine;

namespace LBoLMod.Source.StatusEffects
{
    public sealed class FocusStanceDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FocusStance);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.StatusEffectsBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("icon.png", BepinexPlugin.directorySource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            return new StatusEffectConfig(
                Id: "",
                Index: 0,
                Order: 10,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: false,
                StackActionTriggerLevel: null,
                HasLevel: false,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: true,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
        }
    }

    public sealed class FocusStance: StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            this.React(StanceApplier.StanceApplier.RemoveStance<PowerStance>(unit));
            this.React(StanceApplier.StanceApplier.RemoveStance<CalmStance>(unit));
            this.Count = 3;
            this.ReactOwnerEvent<CardUsingEventArgs>(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            this.Count--;
            if (this.Count == 0)
            {
                base.NotifyActivating();
                this.Count = 3;
                yield return new DrawManyCardAction(1);
            }
            yield break;
        }
    }
}
