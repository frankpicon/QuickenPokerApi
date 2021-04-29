using System;
using System.Collections.Generic;

#nullable disable

namespace Quicken.Poker.Dal.Quicken.Poker.Models
{
    public partial class CardValue
    {
        public CardValue()
        {
            Cards = new HashSet<Card>();
        }

        public decimal CardValueId { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}
