using LBoL.ConfigData;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.Utils;
using LBoLMod.Exhibits;
using LBoLMod.UltimateSkills;


namespace LBoLMod.PlayerUnits
{
    public sealed class PlayerDef : PlayerUnitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ThePlayer);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.playerBatchLoc.AddEntity(this);
        }

        public override PlayerImages LoadPlayerImages()
        {
            return new PlayerImages();
        }

        public override PlayerUnitConfig MakeConfig()
        {
            var reimuConfig = PlayerUnitConfig.FromId("Reimu").Copy();
            var config = new PlayerUnitConfig(
            Id: nameof(ThePlayer),
            ShowOrder: 6,
            Order: 0,
            UnlockLevel: 0,
            ModleName: "",
            NarrativeColor: "#e58c27",
            IsSelectable: true,
            MaxHp: 90,
            InitialMana: new LBoL.Base.ManaGroup() { Red = 2, Blue = 1, White = 1 },
            InitialMoney: 3,
            InitialPower: 30,
            //temp
            UltimateSkillA: nameof(UltimateSkillA),
            UltimateSkillB: reimuConfig.UltimateSkillB,
            ExhibitA: new ExhibitADef().UniqueId,
            ExhibitB: new ExhibitBDef().UniqueId,
            DeckA: reimuConfig.DeckA,
            DeckB: reimuConfig.DeckB,
            DifficultyA: 1,
            DifficultyB: 1
            );
            return config;
        }
    }

    [EntityLogic(typeof(PlayerDef))]
    public sealed class ThePlayer: PlayerUnit
    {

    }
}
