using LBoLMod.Cards;
using System;
using System.Collections.Generic;

namespace LBoLMod.Source.Utils
{
    public static class HaniwaFrontlineUtils
    {
        public static readonly List<Type> CommonSummonTypes = new List<Type>
        {
            typeof(OptionHaniwaAttacker),
            typeof(OptionHaniwaBodyguard),
            typeof(OptionHaniwaDistraction),
            typeof(OptionHaniwaSupport)
        };
    }
}
