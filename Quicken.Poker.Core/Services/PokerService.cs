using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Quicken.Poker.Core.Models;
using Quicken.Poker.Core.ViewModels;
using Quicken.Poker.Dal.Quicken.Poker.Models;


namespace Quicken.Poker.Core.Services
{
    public class PokerService : ServiceBase
    {
        public PokerService()
        {
            this.SetMockRules();
        }

        //TODO: FP-Need to complete implementation async method with await implementation
        public async Task<PlayersViewModel> GetPlayers()
        {

            List<PokerPlayer> players = new List<PokerPlayer>();

            players.Add(GetMockPlayer("Frankie"));
            players.Add(GetMockPlayer("Johnny"));

            PlayersViewModel pvm = new PlayersViewModel() { Players = players };

            return pvm;

        }

        public async Task<WinnerViewModel> GetWinner(PlayersViewModel players)
        {
            if(players.Players.Count==0)
            {
                players.Players.Add(GetMockPlayer("Frankie"));
                players.Players.Add(GetMockPlayer("Johnny"));
            }

            PokerPlayer winner = GetWinner(players.Players);

            WinnerViewModel winnerViewModel = new WinnerViewModel() { WinnerPlayer = winner , Players = players.Players };

            return winnerViewModel;
        }

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

        private void SetMockRules()
        {
            //TODO: FP-implement db lookup of Rules
            //Get all poker rules from the database and build the Poker Rules from data this method simulates the data from the database
            //The HandTypes table contains all the rules. We will build our rules against the data in the Hand Types
            //We will build a collection of the Rules from the PokerRuleFactory

            List<HandType> rules = new List<HandType>();
            HandType ht = new HandType(null) { Value = 9, TypeId = 0 };
            HandType ht2 = new HandType(null) { Value = 8, TypeId = 1 };
            HandType ht3 = new HandType(null) { Value = 7, TypeId = 2 };
            HandType ht4 = new HandType(null) { Value = 6, TypeId = 3 };
            HandType ht5 = new HandType(null) { Value = 5, TypeId = 4 };
            HandType ht6 = new HandType(null) { Value = 4, TypeId = 5 };
            HandType ht7 = new HandType(null) { Value = 3, TypeId = 6 };
            HandType ht8 = new HandType(null) { Value = 2, TypeId = 7 };
            HandType ht9 = new HandType(null) { Value = 1, TypeId = 8 };

            rules.Add(ht);
            rules.Add(ht2);
            rules.Add(ht3);
            rules.Add(ht4);
            rules.Add(ht5);
            rules.Add(ht6);
            rules.Add(ht7);
            rules.Add(ht8);
            rules.Add(ht9);


            foreach (HandType h in rules)
            {
                Quicken.Poker.Core.PokerRuleType pokerRuleType = (PokerRuleType)h.TypeId;
                h.Rule = PokerFactory.Create(pokerRuleType);
                h.Rule.Value = h.Value;
            }

            PokerRuleService.Rules = rules;

        }

        private PokerPlayer GetMockPlayer(string playerName)
        {
            return new PokerPlayer() { Name = playerName, Hand = new PokerHand(GetCards(5)) };
        }


        private PokerHand GetHand()
        {
            //Get Random Cards
            List<Card> Hand = new List<Card>();
            Hand = GetCards(5);
            return new PokerHand(Hand);

        }

        private List<Card> GetCards(int cardQty)
        {
            Random r = new Random();
            List<int> cardValues = new List<int>();
            HashSet<Card> hand = new HashSet<Card>();
            HashSet<string> uniqueCard = new HashSet<string>();

            //Get Random Values from 2-15 this will return the values of Cards 2-10,J,Q,K,A
            while (hand.Count < cardQty)
            {
                int cardValue = r.Next(2, 15);
                const string chars = "HDSC";
                string cardSuit = new string(Enumerable.Repeat(chars, 1)
                  .Select(s => s[r.Next(s.Length)]).ToArray());

                //Check if random is not adding a card with same suite and value
                if (uniqueCard.Add(cardValue.ToString() + cardSuit))
                {
                    CardValue cv = new CardValue() { Value = cardValue };
                    CardSuit cs = new CardSuit() { Name = cardSuit };
                    Card c = new Card() { CardSuit = cs, CardValue = cv };
                    hand.Add(c);
                }

            }





            return hand.ToList();


        }

        private PokerPlayer GetPlayer()
        {
            return new PokerPlayer() { Name = "Frankie" };
        }

        public List<Hand> GetMaxHands(List<Hand> hands)
        {
            var maxHandValue = hands.Max(h => h.HandType.Value);

            return hands.Where(h => h.HandType.Value == maxHandValue).ToList();
        }

        public Hand GetHighCard(List<Hand> hands)
        {
            int highCard = 0;
            Card highHandCard = null;
            Hand playerWHighCard = null;


            foreach (Hand player in hands)
            {
                var playerHighCard = player.Cards.Aggregate((agg, next) => next.CardValue.Value > agg.CardValue.Value ? next : agg);

                if (highHandCard == null || playerHighCard.CardValue.Value > highHandCard?.CardValue?.Value)
                {
                    playerWHighCard = player;
                    highHandCard = playerHighCard;
                }
            }

            return playerWHighCard;

        }

        public Hand GetHighHand(List<Hand> hands)
        {
            var highHandValue = hands.Max(h => h.HandType.Value);
            var highHands = hands.Where(h => h.HandType.Value == highHandValue).ToList();

            if (highHands.Count > 1)
            {
                var hh = GetHighCard(highHands);
                return hh;

                //     return hands.Aggregate((agg, next) =>
                //next.HandType.Value > agg.HandType.Value ? next : agg);

            }
            else
            {
                return highHands[0];
            }



        }

    }



}

 
