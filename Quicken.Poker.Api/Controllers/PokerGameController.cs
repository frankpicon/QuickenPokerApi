using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quicken.Poker.Core;
using Quicken.Poker.Core.Models;
using Quicken.Poker.Core.ViewModels;
using Quicken.Poker.Dal.Quicken.Poker.Models;
using Quicken.Poker.Api.Utilities;
using Quicken.Poker.Core.Services;

namespace Quicken.Poker.Api.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class PokerGameController : ApiControllerBase
    {
        private readonly ILogger<PokerGameController> _logger;

        public PokerGameController(ILogger<PokerGameController> logger, ServiceBase pokerService)
        {
            _pokerService = pokerService;
            _logger = logger;
        }



        ServiceBase _pokerService;




        //[HttpPost]
        //public WinnerViewModel Post([FromBody] PlayersViewModel request)
        //{
        //    List<PokerPlayer> Players = new List<PokerPlayer>();
        //    List<Card> Player1Cards = new List<Card>();
        //    Player1Cards.Add(new Card() { CardSuit = new CardSuit() { Name = "S" }, CardValue = new CardValue() { Value = 14 } });
        //    Player1Cards.Add(new Card() { CardSuit = new CardSuit() { Name = "S" }, CardValue = new CardValue() { Value = 13 } });
        //    Player1Cards.Add(new Card() { CardSuit = new CardSuit() { Name = "S" }, CardValue = new CardValue() { Value = 12 } });
        //    Player1Cards.Add(new Card() { CardSuit = new CardSuit() { Name = "S" }, CardValue = new CardValue() { Value = 11 } });
        //    Player1Cards.Add(new Card() { CardSuit = new CardSuit() { Name = "S" }, CardValue = new CardValue() { Value = 10 } });

        //    List<Card> Player2Cards = new List<Card>();
        //    Player2Cards.Add(new Card() { CardSuit = new CardSuit() { Name = "H" }, CardValue = new CardValue() { Value = 13 } });
        //    Player2Cards.Add(new Card() { CardSuit = new CardSuit() { Name = "H" }, CardValue = new CardValue() { Value = 12 } });
        //    Player2Cards.Add(new Card() { CardSuit = new CardSuit() { Name = "H" }, CardValue = new CardValue() { Value = 11 } });
        //    Player2Cards.Add(new Card() { CardSuit = new CardSuit() { Name = "H" }, CardValue = new CardValue() { Value = 10 } });
        //    Player2Cards.Add(new Card() { CardSuit = new CardSuit() { Name = "H" }, CardValue = new CardValue() { Value = 09 } });

        //    Players.Add(new PokerPlayer() { Name = "Frankie", Hand = new PokerHand(Player1Cards) });
        //    Players.Add(new PokerPlayer() { Name = "Johnny", Hand = new PokerHand(Player2Cards) });

        //    List<WinnerViewModel> a = new List<WinnerViewModel>();
        //    a.Add(new WinnerViewModel() { Players = Players });


        //    PokerPlayer winner = GetWinner(Players);

        //    WinnerViewModel winnerViewModel = new WinnerViewModel() { WinnerPlayer = winner };

        //    return winnerViewModel;

        //}

        private PokerPlayer GetWinner(List<PokerPlayer> players)
        {
            
            foreach (PokerPlayer p in players)
            {

                foreach (HandType r in PokerRuleService.Rules)
                {
                    p.Hand.HandType = r.Rule.CheckRule(p.Hand.Cards);
                    if (p.Hand.HandType != null) { break; };
                }
            }


            var handTypeMaxValue = players.Max(p => p.Hand.HandType.Value);
            var playersWithTopHands = players.Where(p => p.Hand.HandType.Value == handTypeMaxValue).ToList();

            //Check all players that have the same type of Hand
            //If players have same hand then check the Highest Card Value of the hand
            PokerPlayer winner;
            if (playersWithTopHands.Count > 1)
            {
                winner = playersWithTopHands.Aggregate((agg, next) => next.Hand.HandType.HighValue > agg.Hand.HandType.HighValue ? next : agg);
            }
            else //check the value of the highest card
            {
                winner = playersWithTopHands.Aggregate((agg, next) => next.Hand.HandType.Value > agg.Hand.HandType.Value ? next : agg);
            }

            return winner;

        }



        [HttpPost]
        public async Task<GetWinnerViewResult> Post([FromBody] PlayersViewModel request)
        {
            try
            {
                PokerService ps = (PokerService)this._pokerService;
                GetWinnerViewResult result = new GetWinnerViewResult()
                {
                    IsSuccess = true,
                   Winner = await ps.GetWinner(request)
                };
                return result;
            }
            catch (Exception ex)
            {
                return Fail<GetWinnerViewResult>(ex);
            }
        }


        [HttpGet]
        public async Task<GetPlayerViewResult> Get()
        {
            try
            {
                PokerService ps = (PokerService)this._pokerService;
                GetPlayerViewResult result = new GetPlayerViewResult()
                {
                    IsSuccess = true,
                    Players = await ps.GetPlayers()
                };
                return result;
            }
            catch (Exception ex)
            {
                return Fail<GetPlayerViewResult>(ex);
            }
        }

        public class GetWinnerViewResult : ActionResult
        {
            public GetWinnerViewResult()
            {
                
            }
            public  WinnerViewModel Winner { get; set; }
        }

        public class GetPlayerViewResult : ActionResult
        {
            public GetPlayerViewResult()
            {

            }
            public PlayersViewModel Players { get; set; }
        }

    }
}

