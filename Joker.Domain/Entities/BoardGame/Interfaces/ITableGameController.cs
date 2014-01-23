namespace Joker.Domain.Entities.BoardGame.Interfaces
{
    /// <summary>
    /// Interaces that exposes the API for controlling a 
    /// specific tables game flow
    /// </summary>
    public interface ITableGameController
    {
        /// <summary>
        /// Starts a new game
        /// </summary>
        void StartNewGame();

        /// <summary>
        /// Starts a new game round
        /// </summary>
        void StartNewRound();
    }
}