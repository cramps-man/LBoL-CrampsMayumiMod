using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Cards;

namespace LBoLMod.Cards
{
    public abstract class ModMayumiCard : Card
    {
        public string DecorateKeyword(Keyword keyword, string lockey, bool activated)
        {
            string kw = LBoL.Core.Keywords.GetDisplayWord(keyword)?.Name ?? lockey;
            string prop = LocalizeProperty(lockey, true);

            return StringDecorator.Decorate(Battle != null && activated ? $"|d:{kw}: {prop}|" : $"|{kw}|: {prop}").RuntimeFormat(FormatWrapper);
        }
        public string DebutEffect => DecorateKeyword(Keyword.Debut, "Debut", !DebutActive);
        public string UpgradedInteractionTitle => this.LocalizeProperty("UpgradedInteractionTitle", true, false);
        public string InteractionTitle => IsUpgraded && !UpgradedInteractionTitle.IsNullOrEmpty() ? UpgradedInteractionTitle.RuntimeFormat(this.FormatWrapper) : this.LocalizeProperty("InteractionTitle", true).RuntimeFormat(this.FormatWrapper);
    }
}
