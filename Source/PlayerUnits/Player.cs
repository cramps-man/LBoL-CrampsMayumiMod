using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLMod.BattleActions;
using LBoLMod.Cards;
using LBoLMod.Exhibits;
using LBoLMod.UltimateSkills;
using LBoLMod.Utils;
using System.Collections.Generic;


namespace LBoLMod.PlayerUnits
{
    public sealed class MayumiPlayerDef : PlayerUnitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MayumiPlayer);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.playerBatchLoc.AddEntity(this);
        }

        public override PlayerImages LoadPlayerImages()
        {
            var images = new PlayerImages();
            images.AutoLoad("Mayumi", str => ResourceLoader.LoadSprite(str, BepinexPlugin.embeddedSource), str => ResourceLoader.LoadSpriteAsync(str, BepinexPlugin.directorySource));
            return images;
        }

        public override PlayerUnitConfig MakeConfig()
        {
            var config = new PlayerUnitConfig(
            Id: nameof(MayumiPlayer),
            ShowOrder: 6,
            Order: 0,
            UnlockLevel: 0,
            ModleName: "",
            NarrativeColor: "#f4cd39",
            IsSelectable: true,
            MaxHp: 85,
            InitialMana: new LBoL.Base.ManaGroup() { Red = 2, White = 2 },
            InitialMoney: 50,
            InitialPower: 0,
            UltimateSkillA: nameof(FrontlineUltimateSkill),
            UltimateSkillB: nameof(AssignUltimateSkill),
            ExhibitA: new FrontlineExhibitDef().UniqueId,
            ExhibitB: new AssignExhibitDef().UniqueId,
            DeckA: GetFrontlineDeck(),
            DeckB: GetAssignDeck(),
            DifficultyA: 3,
            DifficultyB: 3,
            HasHomeName: true,
            BasicRingOrder: null,
            LeftColor: LBoL.Base.ManaColor.White,
            RightColor: LBoL.Base.ManaColor.Red
            );
            return config;
        }
        private static List<string> GetAssignDeck()
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
                nameof(BasicAssignment),
                nameof(AssignHaniwaCreate)
            };
        }
        private static List<string> GetFrontlineDeck()
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
                nameof(BasicSummon),
                nameof(FrontlineHaniwaCreate)
            };
        }
    }

    [EntityLogic(typeof(MayumiPlayerDef))]
    public sealed class MayumiPlayer: PlayerUnit
    {
        public int StartingHaniwa => 3;
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent(base.TurnStarted, this.OnTurnStarted);
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            var assign = HasExhibit<AssignExhibit>();
            var frontline = HasExhibit<FrontlineExhibit>();
            var hasHaniwa = HaniwaUtils.HasAnyHaniwa(this);
            if (assign || frontline || hasHaniwa)
                yield break;
            if (TurnCounter != 1)
                yield break;

            yield return new GainHaniwaAction(StartingHaniwa, StartingHaniwa, StartingHaniwa);
        }
    }
}
