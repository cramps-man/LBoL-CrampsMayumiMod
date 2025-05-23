﻿using LBoL.Core;
using LBoL.Core.Cards;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.BattleActions
{
    public class CommandEventArgs: GameEventArgs
    {
        public List<Card> CardsToCommand { get; internal set; }
        public UnitSelector Target { get; internal set; }
        public bool ShouldConsumeLoyalty { get; internal set; }
        public string CommandSourceName { get; internal set; }
        public override string GetBaseDebugString()
        {
            return $"Cards = [{string.Join(", ", CardsToCommand.Select(c => c.Name))}], Target = [{Target}], ConsumeLoyalty = [{ShouldConsumeLoyalty}], CommandSource = [{CommandSourceName}]";
        }
    }
}
