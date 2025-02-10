using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.StatusEffects.Keywords;
using LBoLMod.StatusEffects.Localization;
using LBoLMod.Utils;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class FencerKamikazeDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FencerKamikaze);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Type = CardType.Attack;
            cardConfig.TargetType = TargetType.AllEnemies;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { White = 1, Any = 1 };
            cardConfig.UpgradedCost = new ManaGroup() { Any = 2 };
            cardConfig.Damage = 16;
            cardConfig.UpgradedDamage = 21;
            cardConfig.Value1 = 1;
            cardConfig.Keywords = Keyword.Accuracy;
            cardConfig.UpgradedKeywords = Keyword.Accuracy;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Sacrifice) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(FencerKamikazeDef))]
    public sealed class FencerKamikaze : Card
    {
        public override bool CanUse => HaniwaUtils.IsLevelFulfilled<FencerHaniwa>(base.Battle.Player, Value1, HaniwaActionType.Sacrifice);
        public override string CantUseMessage => LocSe.NeedMoreFencer();
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new LoseHaniwaAction(HaniwaActionType.Sacrifice, fencerToLose: Value1);
            yield return AttackAction(selector);
        }
    }
}
