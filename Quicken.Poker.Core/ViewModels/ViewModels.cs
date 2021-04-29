using System;
using System.Collections.Generic;
using System.Text;
using Quicken.Poker.Core.Models;

namespace Quicken.Poker.Core.ViewModels
{

    public class WinnerViewModel
    {
        public PokerPlayer WinnerPlayer { get; set; }
        public List<PokerPlayer> Players { get; set; }
        public WinnerViewModel()
        {

        }
    }

    public class PlayersViewModel
    {
        public List<PokerPlayer> Players { get; set; }
        public PlayersViewModel()
        {
            this.Players = new List<PokerPlayer>();
        }
    }

 
}
