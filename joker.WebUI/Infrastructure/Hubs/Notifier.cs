using System;
using Joker.WebUI.Infrastructure.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Joker.WebUI.Infrastructure.Hubs
{
    /// <summary>
    /// A singleton class that is used by several components
    /// to pass information to the client. Essentially, this singleton 
    /// is a proxy for the hubs since it's using hub contexts
    /// </summary>
    public sealed class Notifier : IGameInitializationNotifier
    {
        #region Singleton pattern

        private static readonly Lazy<Notifier> lazy =
            new Lazy<Notifier>(() => new Notifier());

        public static Notifier Instance
        {
            get { return lazy.Value; }
        }

        private Notifier()
        {
            this._gameHubContext = this.GetHubContext<GameHub>();
        }

        #endregion

        #region private fields

        private readonly IHubContext _gameHubContext;

        #endregion

        #region IGameInitializationNotifier implentation

        /// <summary>
        /// Notify the given client that the table that he joined,
        /// can start the play
        /// </summary>
        /// <param name="playerId">The player to notify</param>
        public void NotifyClientThatGameCanStart(string playerId)
        {
            // call the gameCanStart function on the client side
            this._gameHubContext.Clients.Client(playerId).gameCanStart();
        }

        /// <summary>
        /// Notifies the given client that the table he joined
        /// has started the play
        /// </summary>
        /// <param name="playerId">The player to notify</param>
        public void NotifyClientThatGameHasStarted(string playerId)
        {
            // call the gameStarted function on the client side
            this._gameHubContext.Clients.Client(playerId).gameStarted();
        }

        #endregion

        #region private methods

        /// <summary>
        /// Returns the context of the hub passed as a generic
        /// parameter, via the dependency resolver
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IHubContext GetHubContext<T>() where T : IHub
        {
            return GlobalHost.ConnectionManager.GetHubContext<T>();
        }

        #endregion
    }
}