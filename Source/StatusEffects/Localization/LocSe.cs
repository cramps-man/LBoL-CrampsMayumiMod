using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;

namespace LBoLMod.StatusEffects.Localization
{
    public sealed class LocSeDef : ModStatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LocSe);
        }
    }

    [EntityLogic(typeof(LocSeDef))]
    public sealed class LocSe: StatusEffect
    {
        static LocSe _instance;
        public static LocSe Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Library.CreateStatusEffect<LocSe>();
                return _instance;
            }
        }
        public static string LocalizeProp(string key, bool decorate = false, bool required = true) => Instance.LocalizeProperty(key, decorate, required);
        public static string FrontlineKeyword() => LocalizeProp("FrontlineKeyword");
        public static string AssignKeyword() => LocalizeProp("AssignKeyword");
        public static string NeedMoreFencer() => LocalizeProp("NeedFencerText");
        public static string NeedMoreArcher() => LocalizeProp("NeedArcherText");
        public static string NeedMoreCavalry() => LocalizeProp("NeedCavalryText");
        public static string RequiresHaniwa() => LocalizeProp("RequiresHaniwaText");
    }
}
