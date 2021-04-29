using System;
using System.Collections.Generic;
using System.Text;
using Quicken.Poker.Core.Models;

namespace Quicken.Poker.Core
{
    public class PokerRuleService
    {
        private static int creationCount = 0;
        public static List<HandType> Rules;
        private static readonly PokerRuleService _pokerRuleServiceInstance = new PokerRuleService();

        private PokerRuleService()
        {
            creationCount++;
        }

        public static PokerRuleService GetInstance() => _pokerRuleServiceInstance;

        public int GetCreationCount() => creationCount;
    }

    public enum PokerRuleType
    {
        StraightFlush,
        FourAKind,
        FullHouse,
        Flush,
        Straight,
        ThreeAKind,
        TwoPair,
        OnePair,
        High
    }

    public static class PokerFactory
    {
        public static IPokerRule Create(PokerRuleType pokerRuleType)
        {
            switch (pokerRuleType)
            {
                case PokerRuleType.StraightFlush:
                    return new StraightFlush() { Name = "StraightFlush" };
                case PokerRuleType.FourAKind:
                    return new FourAKind() { Name = "FourAKind" };
                case PokerRuleType.Flush:
                    return new Flush() { Name = "Flush" };
                case PokerRuleType.FullHouse:
                    return new FullHouse() { Name = "FullHouse" };
                case PokerRuleType.High:
                    return new High() { Name = "High" };
                case PokerRuleType.OnePair:
                    return new OnePair() { Name = "OnePair" };
                case PokerRuleType.Straight:
                    return new Straight() { Name = "Straight" };
                case PokerRuleType.ThreeAKind:
                    return new ThreeAKind() { Name = "ThreeAKind" };
                case PokerRuleType.TwoPair:
                    return new TwoPair() { Name = "TwoPair" };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
