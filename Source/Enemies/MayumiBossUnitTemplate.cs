using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.PlayerUnits;
using System.Collections.Generic;

namespace LBoLMod.Enemies
{
    public abstract class MayumiBossUnitTemplate : EnemyUnitTemplate
    {
        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.enemyBatchLoc.AddEntity(this);
        }

        public override EnemyUnitConfig MakeConfig()
        {
            return new EnemyUnitConfig(
                Id: "",
                RealName: false,
                OnlyLore: false,
                BaseManaColor: new List<ManaColor>() { ManaColor.Colorless },
                Order: 10,
                ModleName: nameof(MayumiPlayer),
                NarrativeColor: "#ffff",
                Type: EnemyType.Boss,
                IsPreludeOpponent: true,
                HpLength: null,
                MaxHpAdd: null,
                MaxHp: 250,
                Damage1: 10,
                Damage2: 10,
                Damage3: 10,
                Damage4: 10,
                Power: 1,
                Defend: 15,
                Count1: 1,
                Count2: 2,
                MaxHpHard: 250,
                Damage1Hard: 10,
                Damage2Hard: 10,
                Damage3Hard: 10,
                Damage4Hard: 10,
                PowerHard: 1,
                DefendHard: 15,
                Count1Hard: 1,
                Count2Hard: 2,
                MaxHpLunatic: 250,
                Damage1Lunatic: 10,
                Damage2Lunatic: 10,
                Damage3Lunatic: 10,
                Damage4Lunatic: 10,
                PowerLunatic: 1,
                DefendLunatic: 15,
                Count1Lunatic: 1,
                Count2Lunatic: 2,
                PowerLoot: new MinMax(100, 100),
                BluePointLoot: new MinMax(100, 100),
                Gun1: new List<string> { "Simple1" },
                Gun2: new List<string> { "Simple2" },
                Gun3: new List<string> { "Simple3" },
                Gun4: new List<string> { "Simple4" }
            );
        }
    }
}
