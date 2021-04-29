using System;
using System.Collections.Generic;

#nullable disable

namespace Quicken.Poker.Dal.Quicken.Poker.Models
{
    public partial class Player
    {
        public Player()
        {
            PlayerHands = new HashSet<PlayerHand>();
        }

        public decimal PlayerId { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<PlayerHand> PlayerHands { get; set; }
    }
}
