﻿using System;
using System.Collections.Generic;

namespace Joker.Domain.Entities.BoardGame.DeckOfCards.PlayingCards
{
    public class CardIdentities : List<CardIdentity>
    {
        private static readonly CardIdentities _default = new CardIdentities
                                                              {
                                                                  new CardIdentity('A', "Ace", 1, 1, false),
                                                                  new CardIdentity('2', "Two", 2, 2, false),
                                                                  new CardIdentity('3', "Three", 3, 3, false),
                                                                  new CardIdentity('4', "Four", 4, 4, false),
                                                                  new CardIdentity('5', "Five", 5, 5, false),
                                                                  new CardIdentity('6', "Six", 6, 6, false),
                                                                  new CardIdentity('7', "Seven", 7, 7, false),
                                                                  new CardIdentity('8', "Eight", 8, 8, false),
                                                                  new CardIdentity('9', "Nine", 9, 9, false),
                                                                  new CardIdentity('T', "Ten", 10, 10, false),
                                                                  new CardIdentity('J', "Jack", 10, 11, true),
                                                                  new CardIdentity('Q', "Queen", 10, 12, true),
                                                                  new CardIdentity('K', "King", 10, 13, true),
                                                              };

        private static readonly CardIdentities _jokerCardIdentities = new CardIdentities
                                                              {                                                 
                                                                  new CardIdentity('6', "Six", 6, 5, false),
                                                                  new CardIdentity('7', "Seven", 7, 6, false),
                                                                  new CardIdentity('8', "Eight", 8, 7, false),
                                                                  new CardIdentity('9', "Nine", 9, 8, false),
                                                                  new CardIdentity('T', "Ten", 10, 9, false),
                                                                  new CardIdentity('J', "Jack", 10, 10, true),
                                                                  new CardIdentity('Q', "Queen", 10, 11, true),
                                                                  new CardIdentity('K', "King", 10, 12, true),
                                                                  new CardIdentity('A', "Ace", 1, 13, false),
                                                              };

        public static CardIdentities AceLow()
        {
            return _default;
        }

        /// <summary>
        /// Card identities fot joker gamee
        /// </summary>
        /// <returns></returns>
        public static CardIdentities JokerCardIdentities()
        {
            return _jokerCardIdentities;
        }

        public static CardIdentities AceHigh()
        {
            throw new NotImplementedException("Not currently implemented");
        }
    }
}