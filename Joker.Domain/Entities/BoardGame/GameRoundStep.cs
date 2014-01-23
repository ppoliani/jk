using System.Collections.Generic;
using Joker.Domain.Entities.BoardGame.DeckOfCards;
using Joker.Domain.Entities.Interfaces;

namespace Joker.Domain.Entities.BoardGame
{
    /// <summary>
    /// Instances of this class store information about 
    /// each step of round in the game
    /// </summary>
    public class GameRoundStep
    {
        #region properties

        /// <summary>
        /// The moves of the players in this round round
        /// </summary>
        public IDictionary<IPlayer, ICard> PlayerMoves { get; set; }

        #endregion

    }
}