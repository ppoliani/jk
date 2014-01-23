
using System;
using System.Collections.Generic;
using System.Linq;
using Joker.Domain.Entities.BoardGame.DeckOfCards;
using Joker.Domain.Entities.BoardGame.DeckOfCards.PlayingCards;
using Joker.Domain.Entities.BoardGame.Interfaces;
using Joker.Domain.Entities.Interfaces;

namespace Joker.Domain.Entities.BoardGame
{
    /// <summary>
    /// Represents the dealer that draws the cards
    /// on teach playing table
    /// </summary>
    public class Dealer : IDealer
    {
        #region private fields

        private readonly IDictionary<GameRound, ushort> _cardsForEachRound;
        private readonly PlayingCardDeck _deck;
        private IList<IPlayer> _players;

        #endregion

        #region ctor

        public Dealer()
        {
            this._cardsForEachRound = Config.NumOfCardsInEachRound;
            this._deck = new PlayingCardDeck(CardSuits.Default(), CardIdentities.JokerCardIdentities(), 2, true);
        }

        #endregion

        #region properties

        #endregion

        #region IDealer implementation

        /// <summary>
        /// Assigns cards to the specified card set (i.e. hand)
        /// and returns the list of the assigned cards
        /// </summary>
        /// <param name="currentRound"></param>
        /// <param name="cardSet"></param>
        public IEnumerable<ICard> AssignSet(GameRound currentRound, ICardSet cardSet)
        {
            if (cardSet == null)
            {
                throw new InvalidOperationException("Cannot assign cards to a null cardSet");
            }

            var numOfCardsToAssign = this._cardsForEachRound[currentRound];
            return this._deck.AssignSet(numOfCardsToAssign, cardSet);
        }

        public void StartNewGame(IList<IPlayer> players)
        {
            this._players = players;
        }

        public void StartNewRound(GameRound round)
        {
            this.ResetPlayersHand();
            this.ReturnAllCards();
            this.ShuffleDeck();
            this.DrawCards(round);
        }

        #endregion

        #region public methods

        #endregion

        #region private methods

        /// <summary>
        /// Assign cards to the players
        /// </summary>
        private void DrawCards(GameRound currentRound)
        {
            foreach (var player in this._players)
            {
                player.Hand.Cards = this.AssignSet(currentRound, player.Hand).ToList();
            }
        }

        /// <summary>
        /// Clears the cards in the players' hands
        /// </summary>
        private void ResetPlayersHand()
        {
            foreach (var player in this._players)
            {
                player.ResetHand();
            }
        }

        private void ShuffleDeck()
        {
            this._deck.Shuffle();
        }

        private void ReturnAllCards()
        {
            this._deck.ReturnAllCards();
        }


        #endregion

    }
}