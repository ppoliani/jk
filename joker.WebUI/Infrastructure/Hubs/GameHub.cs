using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Joker.Domain.Entities.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Joker.WebUI.Infrastructure.Hubs
{
    /// <summary>
    /// The main server hub that will respond to clients request
    /// and push some notification accordingly.
    /// This is essentially the communication channel between
    /// the front-end and the back-end.
    /// </summary>
    [HubName("gameHub")]
    public class GameHub : Hub
    {
        #region fields

        private readonly GameController _gameController;

        #endregion

        #region constructor

        public GameHub()
        {
            this._gameController = GameController.Instance;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Client joins the game
        /// </summary>
        /// <returns></returns>
        public void JoinGame(string userName)
        {
            var playerId = Context.ConnectionId;
            this._gameController.JoinGame(userName, playerId);
        }

        /// <summary>
        /// Creates a new table
        /// </summary>
        /// <param name="name">The name if the table</param>
        /// <param name="password">The password of the table (optional)</param>
        public IPlayingTable CreateTable(string name, string password = null)
        {
            var playerId = Context.ConnectionId;
            var table = this._gameController.CreateTable(playerId, name, password);
            Clients.Others.newTableAdded(table);

            return table;
        }

        /// <summary>
        /// Returns all the curent active tables
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetActiveTables()
        {
            return this._gameController.ActiveTables;
        }

        /// <summary>
        /// Return true if the given password matches the table's password
        /// </summary>
        /// <param name="tableId">The id of the table whose password is being verified</param>
        /// <param name="password">The password provided by the user</param>
        /// <returns></returns>
        public bool CheckTablePassword(string tableId, string password)
        {
            return this._gameController.CheckTablePassword(tableId, password);
        }

        /// <summary>
        /// Player joins the specified table
        /// </summary>
        /// <param name="tableId">The id of the table whose password is being verified</param>
        /// <param name="password">The password provided by the user</param>
        public void JoinTable(string tableId, string password)
        {
            var playerId = Context.ConnectionId;
            this._gameController.JoinTable(playerId, tableId, password);
        }

        /// <summary>
        /// Starts the game on the given table
        /// </summary>
        /// <param name="tableId"></param>
        public void StartGameForTable(string tableId)
        {
            this._gameController.StartGameForTable(tableId);
        }

        /// <summary>
        /// Returns true if the user with the current connection id
        /// is already conected to the game
        /// </summary>
        /// <returns></returns>
        public bool IsPlayerConnected()
        {
            var playerId = Context.ConnectionId;

            try
            {
                this._gameController.GetPlayerById(playerId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}

