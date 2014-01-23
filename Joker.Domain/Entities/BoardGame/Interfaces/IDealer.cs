using System.Collections.Generic;
using Joker.Domain.Entities.BoardGame.DeckOfCards;
using Joker.Domain.Entities.Interfaces;

namespace Joker.Domain.Entities.BoardGame.Interfaces
{
    /// <summary>
    /// Interaface that provides API fot delar related
    /// activities
    /// </summary>
    public interface IDealer
    {
        /// <summary>
        /// Assigns cards for the current round to the specified card set (i.e. hand)
        /// and returns the list of the assigned cards
        /// </summary>
        /// <param name="currentRound"></param>
        /// <param name="cardSet"></param>
        IEnumerable<ICard> AssignSet(GameRound currentRound, ICardSet cardSet);

        /// <summary>
        /// Starts a new game
        /// </summary>
        void StartNewGame(IList<IPlayer> players);

        /// <summary>
        /// Start the current round
        /// </summary>
        void StartNewRound(GameRound round);
    }
}