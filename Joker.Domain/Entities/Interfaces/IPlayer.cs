using System;
using System.Collections.Generic;
using Joker.Domain.Entities.BoardGame;
using Joker.Domain.Entities.BoardGame.DeckOfCards;

namespace Joker.Domain.Entities.Interfaces
{
    public interface IPlayer
    {
        /// <summary>
        /// Unique identifier for the player
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// user name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The table that is currently on
        /// </summary>
        IPlayingTable CurrentTable { get; set; }

        /// <summary>
        /// The chair that players sits on
        /// </summary>
        char CurrentChair { get; set; }

        /// <summary>
        /// The current players hand
        /// </summary>
        ICardSet Hand { get; set; }

        /// <summary>
        /// Clears the current cards in player's hand
        /// </summary>
        void ResetHand();
    }
}