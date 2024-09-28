using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards;
using LBoLMod.BattleActions;
using LBoLMod.StatusEffects;
using LBoLMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBoLMod.Cards
{
    public abstract class ModFrontlineOptionCard: Card
    {
        public int AvailableFencer => HaniwaUtils.GetHaniwaLevel<FencerHaniwa>(base.Battle.Player);
        public int AvailableArcher => HaniwaUtils.GetHaniwaLevel<ArcherHaniwa>(base.Battle.Player);
        public int AvailableCavalry => HaniwaUtils.GetHaniwaLevel<CavalryHaniwa>(base.Battle.Player);
        public virtual int SelectRequireFencer => 0;
        public virtual int SelectRequireArcher => 0;
        public virtual int SelectRequireCavalry => 0;
        public virtual Type CardTypeToSpawn => null;
        public int NumberToSpawn { get; set; } = 1;
        public bool FulfilsRequirement => HaniwaUtils.IsLevelFulfilled(base.Battle.Player, HaniwaActionType.Sacrifice, SelectRequireFencer, SelectRequireArcher, SelectRequireCavalry);

        public List<Card> GetCardsToSpawn()
        {
            List<Card> cards = new List<Card>();
            foreach (var i in Enumerable.Range(0, NumberToSpawn))
                cards.Add(Library.CreateCard(CardTypeToSpawn));
            return cards;
        }
    }
}
