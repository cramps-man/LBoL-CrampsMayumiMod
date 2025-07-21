using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace LBoLMod.Enemies
{
    public sealed class MayumiBossGroup : EnemyGroupTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MayumiBossUnit);
        }

        public override EnemyGroupConfig MakeConfig()
        {
            return new EnemyGroupConfig(
                Id: "",
                Hidden: false,
                Environment: null,
                IsSub: false,
                Subs: new List<string>() { },
                Name: nameof(MayumiBossUnit),
                FormationName: VanillaFormations.Single,
                Enemies: new List<string>() { nameof(MayumiBossUnit) },
                EnemyType: EnemyType.Boss,
                DebutTime: 1f,
                RollBossExhibit: true,
                PlayerRoot: new Vector2(-4f, 0.5f),
                PreBattleDialogName: "",
                PostBattleDialogName: ""
            );

        }
    }
}
