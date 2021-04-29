using System;
using System.Collections.Generic;

#nullable disable

namespace Quicken.Poker.Dal.Quicken.Poker.Models
{
    public partial class CardSuit
    {
        public CardSuit()
        {
            Cards = new HashSet<Card>();
        }

        public decimal CardSuitId { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}
