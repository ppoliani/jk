namespace Joker.WebUI.Infrastructure.Interfaces
{
    /// <summary>
    /// Suports functionality related to the game 
    /// initialization stage
    /// </summary>
    public interface IGameInitializationNotifier
    {
        /// <summary>
        /// Notifies the given client that the table that he joined,
        /// can start the play
        /// </summary>
        /// <param name="playerId">The player to notify</param>
        void NotifyClientThatGameCanStart(string playerId);

        /// <summary>
        /// Notifies the given client that the table he joined
        /// has started the play
        /// </summary>
        /// <param name="playerId">The player to notify</param>
        void NotifyClientThatGameHasStarted(string playerId);
    }
}