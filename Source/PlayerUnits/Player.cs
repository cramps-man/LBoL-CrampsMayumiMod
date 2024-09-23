using LBoL.ConfigData;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.Cards;
using LBoLMod.Exhibits;
using LBoLMod.UltimateSkills;
using System.Collections.Generic;


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
            var config = new PlayerUnitConfig(
            Id: nameof(ThePlayer),
            ShowOrder: 6,
            Order: 0,
            UnlockLevel: 0,
            ModleName: "",
            NarrativeColor: "#f4cd39",
            IsSelectable: true,
            MaxHp: 85,
            InitialMana: new LBoL.Base.ManaGroup() { Red = 2, White = 2 },
            InitialMoney: 50,
            InitialPower: 30,
            UltimateSkillA: nameof(UltimateSkillA),
            UltimateSkillB: nameof(UltimateSkillB),
            ExhibitA: new ExhibitADef().UniqueId,
            ExhibitB: new ExhibitBDef().UniqueId,
            DeckA: GetDeckA(),
            DeckB: GetDeckB(),
            DifficultyA: 1,
            DifficultyB: 1
            );
            return config;
        }
        private static List<string> GetDeckA()
        {
            return new List<string> {
                nameof(Shoot),
                nameof(Shoot),
                nameof(Boundary),
                nameof(Boundary),
                nameof(BasicAttackR),
                nameof(BasicAttackR),
                nameof(BasicBlockW),
                nameof(BasicBlockW),
                nameof(BasicBlockW),
                nameof(ArcherPrepVolley)
            };
        }
        private static List<string> GetDeckB()
        {
            return new List<string> {
                nameof(Shoot),
                nameof(Shoot),
                nameof(Boundary),
                nameof(Boundary),
                nameof(BasicAttackW),
                nameof(BasicAttackW),
                nameof(BasicBlockR),
                nameof(BasicBlockR),
                nameof(BasicBlockR),
                nameof(FencerKamikaze),
            };
        }
    }

    [EntityLogic(typeof(PlayerDef))]
    public sealed class ThePlayer: PlayerUnit
    {
    }
}
