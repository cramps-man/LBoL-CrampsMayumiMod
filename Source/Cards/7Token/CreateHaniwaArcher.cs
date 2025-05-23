﻿using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CreateHaniwaArcherDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CreateHaniwaArcher);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.IsPooled = false;
            cardConfig.HideMesuem = true;
            cardConfig.Type = CardType.Skill;
            cardConfig.Value1 = 2;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CreateHaniwaArcherDef))]
    public sealed class CreateHaniwaArcher:OptionCard
    {
        public int CurrentArcher => HaniwaUtils.GetHaniwaLevel<ArcherHaniwa>(base.Battle.Player);
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            yield return new GainHaniwaAction(archerToGain: Value1);
        }
    }
}
