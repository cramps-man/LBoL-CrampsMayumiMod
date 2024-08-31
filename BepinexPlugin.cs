using BepInEx;
using HarmonyLib;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Reflection;
using UnityEngine;


namespace LBoLMod
{
    [BepInPlugin(LBoLMod.PInfo.GUID, LBoLMod.PInfo.Name, LBoLMod.PInfo.version)]
    [BepInDependency(LBoLEntitySideloader.PluginInfo.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(AddWatermark.API.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("LBoL.exe")]
    public class BepinexPlugin : BaseUnityPlugin
    {

        private static readonly Harmony harmony = LBoLMod.PInfo.harmony;

        internal static BepInEx.Logging.ManualLogSource log;

        internal static TemplateSequenceTable sequenceTable = new TemplateSequenceTable();

        internal static IResourceSource embeddedSource = new EmbeddedSource(Assembly.GetExecutingAssembly());
        internal static DirectorySource directorySource = new DirectorySource(LBoLMod.PInfo.GUID, "");

        internal static BatchLocalization cardBatchLoc = new BatchLocalization(directorySource, typeof(CardTemplate), "cards");
        internal static BatchLocalization playerBatchLoc = new BatchLocalization(directorySource, typeof(PlayerUnitTemplate), "playerUnit");
        internal static BatchLocalization unitModelBatchLoc = new BatchLocalization(directorySource, typeof(UnitModelTemplate), "unitModel");
        internal static BatchLocalization exhibitBatchLoc = new BatchLocalization(directorySource, typeof(ExhibitTemplate), "exhibit");
        internal static BatchLocalization StatusEffectsBatchLoc = new BatchLocalization(directorySource, typeof(StatusEffectTemplate), "statusEffect");

        private void Awake()
        {
            log = Logger;

            // very important. Without this the entry point MonoBehaviour gets destroyed
            DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;

            EntityManager.RegisterSelf();

            harmony.PatchAll();

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(AddWatermark.API.GUID))
                WatermarkWrapper.ActivateWatermark();
        }

        private void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchSelf();
        }


    }
}
