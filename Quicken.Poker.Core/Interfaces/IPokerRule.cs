using System;
using System.Collections.Generic;
using Quicken.Poker.Dal.Quicken.Poker.Models;

namespace Quicken.Poker.Core
{
    public interface IPokerRule
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public bool IsValid { get; set; }
        public Card HighCard { get; set; }
        public int HighValue { get; set; }
        public IPokerRule CheckRule(List<Card> cards);
        
    }
}
