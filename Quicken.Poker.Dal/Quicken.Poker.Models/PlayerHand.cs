using System;
using System.Collections.Generic;

#nullable disable

namespace Quicken.Poker.Dal.Quicken.Poker.Models
{
    public partial class PlayerHand
    {
        public decimal PlayerHandId { get; set; }
        public decimal PlayerId { get; set; }
        public decimal CardId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Deleted { get; set; }

        public virtual Card Card { get; set; }
        public virtual Player Player { get; set; }
    }
}
