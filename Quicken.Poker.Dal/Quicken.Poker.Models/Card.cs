using System;
using System.Collections.Generic;

#nullable disable

namespace Quicken.Poker.Dal.Quicken.Poker.Models
{
    public partial class Card
    {
        public Card()
        {
            PlayerHands = new HashSet<PlayerHand>();
        }

        public decimal CardId { get; set; }
        public decimal CardSuitId { get; set; }
        public decimal CardValueId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Deleted { get; set; }

        public virtual CardSuit CardSuit { get; set; }
        public virtual CardValue CardValue { get; set; }
        public virtual ICollection<PlayerHand> PlayerHands { get; set; }
    }
}
