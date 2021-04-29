using System;
using System.Collections.Generic;
using System.Text;
using Quicken.Poker.Dal.Quicken.Poker.Models;
using System.Linq;

namespace Quicken.Poker.Core.Models
{
    public class StraightFlush : IPokerRule
    {
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public Card HighCard { get; set; }
        public int HighValue { get; set; }
        public IPokerRule CheckRule(List<Card> cards)
        {
            IPokerRule straightFlush = null;

            //TODO: FP-check if you need the final check if has all numeric since we may assign all values to each card as numeric
            this.IsValid = HasSameSuit(cards) && AllConsecutiveValue(cards) && HasAllNumericValue(cards);

            SetHighCard(cards);

            if (this.IsValid) 
            { 
                SetHighCard(cards);
                straightFlush = new StraightFlush() { HighCard = this.HighCard, HighValue = this.HighValue, Name = this.Name, Value = this.Value };
            }

            return straightFlush;
        }

        private void SetHighCard(List<Card> cards)
        {
            this.HighCard = cards.Aggregate((agg, next) => next.CardValue.Value > agg.CardValue.Value ? next : agg);
            this.HighValue = this.HighCard.CardValue.Value;
        }

        private bool HasSameSuit(List<Card>cards)
        {
            return (cards.All(c => c.CardSuit.Name == cards[0].CardSuit.Name)) ;
        }
        private bool HasAllNumericValue(List<Card>cards)
        {
            int hasNum = 0;
            return cards.All(c => int.TryParse(c.CardValue.Value.ToString(), out hasNum));

        }
        private bool AllConsecutiveValue(List<Card> cards)
        {
            var sortedCards = cards.OrderBy(c => c.CardValue.Value).ToList();

            return   !sortedCards.Select((c, v) =>  c.CardValue.Value  - v).Distinct().Skip(1).Any();
        }

        private bool AllSameSuit(List<Card> cards)
        {
            //check if all cards are of the same suit
            if (cards.Any(o => o != cards[0]))
            { return false; }

            return true;
        }
    }


    public class FourAKind : IPokerRule
    {
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public Card HighCard { get; set; }
        public int HighValue { get; set; }
        public IPokerRule CheckRule(List<Card> cards)
        {
            IPokerRule fourAKind = null;

            if (HasFourSameValue(cards))
            {
                fourAKind = new FourAKind() { HighCard = this.HighCard, HighValue = this.HighValue, Name = this.Name, Value = this.Value };
            }

            return fourAKind;
        }

        private bool HasFourSameValue(List<Card> cards)
        {
            //Check cards if any card is repeated 4 times and return the value of the card
            this.HighValue = cards.GroupBy(x => x.CardValue.Value)
              .Where(g => g.Count() == 4)
              .Select(y => y.Key).FirstOrDefault();

            //Set one of the Cards that has the 4 of a kind as the High Card.
            if (this.HighValue > 0) { this.HighCard = cards.Where(c => c.CardValue.Value == this.HighValue).FirstOrDefault(); };

            return this.HighValue > 0 ;
        }


    }

    public class FullHouse : IPokerRule
    {

        public string Name { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public Card HighCard { get; set; }
        public int HighValue { get; set; }
        public IPokerRule CheckRule(List<Card> cards)
        {
            IPokerRule fullHouse = null;

            if (HasThreeSameValue(cards) && HasPair(cards))
            {
                fullHouse = new FullHouse() { HighCard = this.HighCard, HighValue = this.HighValue, Name = this.Name, Value = this.Value };
            }
            return fullHouse;
        }
        private bool HasPair(List<Card> cards)
        {
            //Check cards if any card is repeated 2 times and return the value of the card
            var pairValue = cards.GroupBy(x => x.CardValue.Value)
              .Where(g => g.Count() == 2)
              .Select(y => y.Key).FirstOrDefault();

            return pairValue > 0 ;
        }


        private bool HasThreeSameValue(List<Card> cards)
        {
            //Check cards if any card is repeated 3 times and return the value of the card
            this.HighValue = cards.GroupBy(x => x.CardValue.Value)
              .Where(g => g.Count() == 3)
              .Select(y => y.Key).FirstOrDefault();

            //Set one of the Cards that has the 3 of a kind as the High Card.
            //Set the High Value the total value of the 3 cards.
            if (this.HighValue > 0) { 
                this.HighCard = cards.Where(c => c.CardValue.Value == this.HighValue).FirstOrDefault();
                this.HighValue = this.HighValue * 3; 
            }

            return this.HighValue > 0;
        }

    }


    public class Flush : IPokerRule
    {

        public string Name { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public Card HighCard { get; set; }
        public int HighValue { get; set; }
        public IPokerRule CheckRule(List<Card> cards)
        {
            IPokerRule flush = null;

            if (HasSameSuit(cards) && !AllConsecutiveValue(cards))
            {
                flush = new Flush() { HighCard = this.HighCard, HighValue = this.HighValue, Name = this.Name, Value = this.Value };
            }
            return flush;
        }

        private bool HasSameSuit(List<Card> cards)
        {
            var allSame = (cards.All(c => c.CardSuit.Name == cards[0].CardSuit.Name));
            if (allSame) { SetHighValue(cards); }
            return allSame;
        }

        private void SetHighValue(List<Card> cards)
        {
            this.HighCard = cards.Aggregate((agg, next) => next.CardValue.Value > agg.CardValue.Value ? next : agg);
            this.HighValue = cards.Sum(c=>c.CardValue.Value);
        }

        private bool AllConsecutiveValue(List<Card> cards)
        {
            var sortedCards = cards.OrderBy(c => c.CardValue.Value).ToList();

            return !sortedCards.Select((c, v) => c.CardValue.Value - v).Distinct().Skip(1).Any();
        }
    }



    public class Straight : IPokerRule
    {
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public Card HighCard { get; set; }
        public int HighValue { get; set; }
        public IPokerRule CheckRule(List<Card> cards)
        {
            IPokerRule straight = null;

            if (!HasSameSuit(cards) && AllConsecutiveValue(cards))
            {
                straight = new Straight() { HighCard = this.HighCard, HighValue = this.HighValue, Name = this.Name, Value = this.Value };
            }
            return straight;
        }

        private bool HasSameSuit(List<Card> cards)
        {
            var allSame = (cards.All(c => c.CardSuit.Name == cards[0].CardSuit.Name));
            if (allSame) { SetHighValue(cards); }
            return allSame;
        }

        private void SetHighValue(List<Card> cards)
        {
            this.HighCard = cards.Aggregate((agg, next) => next.CardValue.Value > agg.CardValue.Value ? next : agg);
            this.HighValue = this.HighCard.CardValue.Value;
        }

        private bool AllConsecutiveValue(List<Card> cards)
        {
            var sortedCards = cards.OrderBy(c => c.CardValue.Value).ToList();

            return !sortedCards.Select((c, v) => c.CardValue.Value - v).Distinct().Skip(1).Any();
        }

    }

    public class ThreeAKind : IPokerRule
    {
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public Card HighCard { get; set; }
        public int HighValue { get; set; }
        public IPokerRule CheckRule(List<Card> cards)
        {
            IPokerRule threeKind = null;

            if (HasThreeSameValue(cards))
            {
                threeKind = new ThreeAKind() { HighCard = this.HighCard, HighValue = this.HighValue, Name = this.Name, Value = this.Value };
            }
            return threeKind;


        }
        private bool HasThreeSameValue(List<Card> cards)
        {
            //Check cards if any card is repeated 4 times and return the value of the card
            this.HighValue = cards.GroupBy(x => x.CardValue.Value)
              .Where(g => g.Count() == 3)
              .Select(y => y.Key).FirstOrDefault();

            //Set one of the Cards that has the 4 of a kind as the High Card.
            if (this.HighValue > 0) { this.HighCard = cards.Where(c => c.CardValue.Value == this.HighValue).FirstOrDefault(); };

            return this.HighValue > 0;
        }
    }

    public class  TwoPair : IPokerRule
    {
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public Card HighCard { get; set; }
        public int HighValue { get; set; }
        public IPokerRule CheckRule(List<Card> cards)
        {
            IPokerRule twoPair = null;

            if (HasTwoPair(cards))
            {
                twoPair = new TwoPair() { HighCard = this.HighCard, HighValue = this.HighValue, Name = this.Name, Value = this.Value };
            }
            return twoPair;
        }

        private bool HasTwoPair(List<Card> cards)
        {
            //Check cards if any card is repeated 2 times and return the value of the card
            this.HighValue = cards.GroupBy(x => x.CardValue.Value)
              .Where(g => g.Count() == 2)
              .Select(y => y.Key).FirstOrDefault();

            //Check cards for next pair and return its value
            int nextPairHighValue = cards.Where(c => c.CardValue.Value != this.HighValue)
                .AsEnumerable().GroupBy(x => x.CardValue.Value)
              .Where(g => g.Count() == 2)
              .Select(y => y.Key).FirstOrDefault();

            if (this.HighValue > 0 && nextPairHighValue > 0 ) {

                this.HighValue = this.HighValue > nextPairHighValue ? this.HighValue : nextPairHighValue;
                this.HighCard = cards.Where(c => c.CardValue.Value == this.HighValue).FirstOrDefault();

                return true;
            }else { 
                return false; }

        }
    }

    public class OnePair : IPokerRule
    {
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public Card HighCard { get; set; }
        public int HighValue { get; set; }
        public IPokerRule CheckRule(List<Card> cards)
        {
            IPokerRule onePair = null;

            if (HasOnePair(cards))
            {
                onePair = new OnePair() { HighCard = this.HighCard, HighValue = this.HighValue, Name = this.Name, Value = this.Value };
            }

            return onePair;
        }

        private bool HasOnePair(List<Card> cards)
        {
            //Check cards if any card is repeated 2 times and return the value of the card
            this.HighValue = cards.GroupBy(x => x.CardValue.Value)
              .Where(g => g.Count() == 2)
              .Select(y => y.Key).FirstOrDefault();

            //Check cards for next pair if so then this deck has 2 two pairs not one
            int nextPairHighValue = cards.Where(c => c.CardValue.Value != this.HighValue)
               .AsEnumerable().GroupBy(x => x.CardValue.Value)
               .Where(g => g.Count() == 2)
               .Select(y => y.Key).FirstOrDefault();

            if (this.HighValue > 0 && nextPairHighValue == 0)
            {  
                this.HighCard = cards.Where(c => c.CardValue.Value == this.HighValue).FirstOrDefault();

                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public class High : IPokerRule
    {
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public Card HighCard { get; set; }
        public int HighValue { get; set; }
        public IPokerRule CheckRule(List<Card> cards)
        {
            IPokerRule high = null;
            if(HasOnlyHighCard(cards))
            {
                high = new High() { HighCard = this.HighCard, HighValue = this.HighValue, Name = this.Name, Value = this.Value };
            }
            return high;
        }

        public bool HasOnlyHighCard(List<Card> cards)
        {
            if (!HasAnyPair(cards) && !AllConsecutiveValue(cards)) {
                SetHighCard(cards);
            };

            return this.HighValue > 0 ;
        }
        private void SetHighCard(List<Card> cards)
        {
            //Set the HighCard as the card with the highest value
            //Set the HighValue as the total value of all the cards , this can determine the winner if other players have a high card
            this.HighCard = cards.Aggregate((agg, next) => next.CardValue.Value > agg.CardValue.Value ? next : agg);
            this.HighValue = cards.Sum(c => c.CardValue.Value);
        }

        private bool HasAnyPair(List<Card> cards)
        {
            //Check cards if any card is repeated at least 2 times and return the value of the card
            var pairValue = cards.GroupBy(x => x.CardValue.Value)
              .Where(g => g.Count() >= 2)
              .Select(y => y.Key).FirstOrDefault();

            return pairValue > 0;
        }

        private bool AllConsecutiveValue(List<Card> cards)
        {
            var sortedCards = cards.OrderBy(c => c.CardValue.Value).ToList();

            return !sortedCards.Select((c, v) => c.CardValue.Value - v).Distinct().Skip(1).Any();
        }



    }


        public class PokerPlayer
    {
        public string Name { get; set; }

        public PokerHand Hand { get; set; } 

    }

    public class PokerHand
    {
        public List<Card> Cards { get; set; }

        public IPokerRule HandType { get; set; }
        
        public PokerHand(List<Card> cards)
        {
            this.Cards = cards;
        }
    }



    public class Hand
    {
        public List<Card> Cards { get; set; }

        public HandType HandType { get; set; }

        public string Player { get; set; }
    }

    public class HandType
    {
        public int TypeId { get; set; }
        public IPokerRule Rule { get; set; }

        public int Value { get; set; }
        public string Name { get { return this.Rule.Name; } }


        public HandType(IPokerRule rule)
        {
            this.Rule = rule;
        }

        public HandType()
        {

        }
    }

    public class PokerRules
    {
        public List<HandType> Rules { get; set; }

        public PokerHand GetHandType(List<Card> cards)
        {
            FourAKind k4 = new FourAKind() { Name = "4k", Value = 8 };
            var k = k4.CheckRule(cards);

            foreach (HandType h in Rules)
            {
                var a = h.Rule;
            }

            return null;

        }


    }

}
