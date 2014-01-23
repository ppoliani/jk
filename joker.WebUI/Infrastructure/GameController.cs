using System;
using System.Collections.Generic;
using System.Linq;
using Joker.Domain.Entities;
using Joker.Domain.Entities.Interfaces;
using Joker.Domain.Exceptions;
using Joker.WebUI.Infrastructure.Interfaces;

namespace Joker.WebUI.Infrastructure
{
    /// <summary>
    /// The main controller that is responsible for the whole game.
    /// Follows the singleton and mediator pattern design
    /// </summary>
    public sealed class GameController
    {
        #region Singleton pattern

        private static readonly Lazy<GameController> lazy =
            new Lazy<GameController>(() => new GameController());

        public static GameController Instance
        {
            get { return lazy.Value; }
        }

        private GameController()
        {
            this._gamePlayers = new List<IPlayer>();
            this._activeTables = new List<IPlayingTable>();
        }

        #endregion

        #region private fields

        private readonly IList<IPlayer> _gamePlayers;
        private readonly IList<IPlayingTable> _activeTables;
        private INotifier _notifier;

        #endregion

        #region properties

        public IList<IPlayer> GamePlayers
        {
            get { return this._gamePlayers; }
        }

        public IList<IPlayingTable> ActiveTables
        {
            get { return this._activeTables; }
        }

        /// <summary>
        /// The notifier that will act as a proxy
        /// between the this class and the client side.
        /// Notifier is a singleton class but we still want
        /// to inject that reference, to make testing easier
        /// </summary>
        public INotifier Notifier
        {
            set { this._notifier = value; }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Add the player to the game
        /// </summary>
        /// <param name="name">The user name entered by the user</param>
        /// <param name="playerId">The connection id of the player</param>
        public void JoinGame(string name, string playerId)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("The player needs to provide a name");
            }

            var newPlayer = new Player(name)
                                {
                                    Id = playerId
                                };
            this.GamePlayers.Add( newPlayer );
        }

        public void LeaveGame(string playerId)
        {
            var playerToRemove = this.GetPlayerById(playerId);
            this.GamePlayers.Remove(playerToRemove);
        }

        /// <summary>
        /// Creates a new table with the given name
        /// </summary>
        /// <param name="playerId">The id of the player who created the table</param>
        /// <param name="name">The name of the table</param>
        /// <param name="password">The password of the table (optional)</param>
        /// <returns>The newly created table</returns>
        public IPlayingTable CreateTable(string playerId, string name, string password = null)
        {
            // should never be true. Validation should be done
            // on the above layers
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("The name of the table cannot be empty");
            }

            var newTable = new PlayingTable(playerId, name, password)
                               {
                                   Id = Guid.NewGuid().ToString()
                               };

            // subscribe to game can start event
            newTable.GameCanStart += this.GameCanStart;

            this.ActiveTables.Add(newTable);

            return newTable;
        }

        /// <summary>
        /// Adds the specified player to the given table
        /// </summary>
        public void JoinTable(string playerId, string tableId, string password = null)
        {
            if (playerId == null || tableId == null )
            {
                throw new InvalidOperationException("Player and table instances must be specified");
            }

            // has already joined?
            if (this.HasPlayerJoinedTheTable(playerId,tableId))
            {
                throw new PlayerHasAlreadyJoinedTableException(String.Format("Player {0} has already joined the table {1}", playerId, tableId) );
            }

            var tableToJoin = this.GetTableById(tableId);
            var player = this.GetPlayerById(playerId);

            if (tableToJoin.Authenticate(password))
            {
                // add to the specified table
                tableToJoin.AddPlayer(player);
            }
            else
            {
                throw new WrongTablePasswordEntered("Player " + playerId + " entered wrong table password");
            }          
        }

        /// <summary>
        /// The given player choses the specified chair
        /// </summary>
        /// <param name="playerId">The players id</param>
        /// <param name="chairNum">
        ///     Chair is one of the following char options: 
        ///         N for North
        ///         E for East,
        ///         S for South,
        ///         W for West
        /// </param>
        public void PickChair(string playerId, char chairNum)
        {
            var player = this.GetPlayerById(playerId);
            var table = player.CurrentTable;

            if (table == null)
            {
                throw new InvalidOperationException("The player hasn't joined any table");
            }

            table.PickChair(player, chairNum);
        }

        /// <summary>
        /// The oposite of joining a table. The player 
        /// leaves the table
        /// </summary>
        /// <param name="playerId">The id of the player</param>
        public void LeaveTable(string playerId)
        {
            var player = this.GetPlayerById(playerId);
            var table = player.CurrentTable;
            table.RemovePlayer(player);
        }

        /// <summary>
        /// Returns the player from the list of the players
        /// with the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IPlayer GetPlayerById(string id)
        {
            var player = this.GamePlayers.FirstOrDefault(x => x.Id.ToString() == id);

            if (player == null)
            {
                throw new PlayerNotFoundException(String.Format("The player with the id {0} was not found", id));
            }

            return player;
        }

        /// <summary>
        /// Returns the table with the specified id
        /// </summary>
        /// <returns></returns>
        public IPlayingTable GetTableById(string id)
        {
            var table = this.ActiveTables.FirstOrDefault(x => x.Id.ToString() == id);

            if (table == null)
            {
                throw new TableNotFoundException("There is no table with the id: " + id);
            }

            return table;
        }

        /// <summary>
        /// Return true if the given password matches the table's password
        /// </summary>
        /// <param name="tableId">The id of the table whose password is being verified</param>
        /// <param name="password">The password provided by the user</param>
        /// <returns></returns>
        public bool CheckTablePassword(string tableId, string password)
        {
            var table = this.GetTableById(tableId);
            return table.Authenticate(password);
        }

        /// <summary>
        /// This method ma the beggining of the game
        /// for the specified table
        /// </summary>
        public void StartGameForTable(string tableId)
        {
            var tablePlayers = this.GetTableById(tableId).Players;

            foreach (var player in tablePlayers)
            {
                this._notifier.NotifyClientThatGameHasStarted(player.Id);                
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Checks if the given player has already joined the 
        /// given table
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="tableId"></param>
        /// <returns></returns>
        private bool HasPlayerJoinedTheTable(string playerId, string tableId)
        {
            return GetTableById(tableId).HasPlayerJoinedTheTable(playerId);
        }

        /// <summary>
        /// Notifies the given players that the game can start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tableCanStartEventArgs"></param>
        private void GameCanStart(object sender, TableCanStartEventArgs tableCanStartEventArgs)
        {
            // notify all the players 
            tableCanStartEventArgs.PlayersToBeNotified
                .ToList()
                .ForEach(x => this._notifier.NotifyClientThatGameCanStart(x));
        }

        #endregion
    }
}