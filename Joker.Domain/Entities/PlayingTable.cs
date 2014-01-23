using System;
using System.Collections.Generic;
using System.Linq;
using Joker.Domain.Entities.BoardGame;
using Joker.Domain.Entities.BoardGame.Interfaces;
using Joker.Domain.Entities.Interfaces;
using Joker.Domain.Exceptions;

namespace Joker.Domain.Entities
{
    #region Inner members

    public class TableCanStartEventArgs : EventArgs
    {
        public readonly IList<string> PlayersToBeNotified;

        public TableCanStartEventArgs(IList<string> playersToBeNotified)
        {
            this.PlayersToBeNotified = playersToBeNotified;
        }
    }

    #endregion

    /// <summary>
    /// Represents a playing table
    /// </summary>
    public class PlayingTable : IPlayingTable, ITableGameController
    {
        #region static 

        public static ushort MAX_NUMBER_OF_PLAYERS = 4;

        #endregion

        #region private fields

        private readonly IDealer _dealer;

        // The password of the current table
        private readonly string _password;

        private GameRound _currentRound;

        private readonly IDictionary<string, char> _playerChairs;

        #endregion

        #region contrsuctors

        public PlayingTable(string creator, string name, string password = null)
        {
            this.Name = name;
            this.Creator = creator;
            this._password = password;
            this._dealer = new Dealer();
            this.Players = new List<IPlayer>();
            this.UserScores = new Dictionary<IPlayer, IRoundScore>();
            this.GameRoundsInfo = new Dictionary<GameRound, GameRoundInfo>();
            this._playerChairs = new Dictionary<string, char>();
        }

        #endregion

        #region properties

        public string Id { get; set; }

        /// <summary>
        /// Gets the creator (i.e. player id) of the table
        /// </summary>
        public string Creator { get; set; }

        public string Name { get; set; }

        public IList<IPlayer> Players { get; set; }

        /// <summary>
        /// Indicates if the current table has a password assigned
        /// </summary>
        public bool HasPassword
        {
            get { return !String.IsNullOrEmpty(this._password); }
        }

        public IDictionary<IPlayer, IRoundScore> UserScores { get; set; }

        public GameRound CurrentRound
        {
            get { return this._currentRound; }
        }

        public IDictionary<GameRound, GameRoundInfo> GameRoundsInfo { get; set; }

        #endregion

        #region Events

        public event EventHandler<TableCanStartEventArgs> GameCanStart;

        #endregion

        #region IPlayingTable implementation

        public void AddPlayer(IPlayer player)
        {
            if ( this.IsFull() )
            {
                throw new TableIsFullException();
            }

            this.Players.Add(player);
            player.CurrentTable = this;

            // can the game start?
            if (this.IsFull())
            {
                this.OnGameCanStart();
            }
        }

        public bool Authenticate(string password)
        {
            return String.IsNullOrEmpty(this._password) 
                || this._password == password;
        }

        /// <summary>
        /// The given player choses the specified chair
        /// </summary>
        /// <param name="player">The players id</param>
        /// <param name="chair">
        ///     Chair is one of the following char options: 
        ///         N for North
        ///         E for East,
        ///         S for South,
        ///         W for West
        /// </param>
        public void PickChair(IPlayer player, char chair)
        {
            var activePlayer = GetPlayerById(player);

            if (this.IsChairOccupied(chair))
            {
                throw new InvalidOperationException(String.Format("The chair {0} is taken", chair));
            }

            activePlayer.CurrentChair = chair;
            // store locally
            this._playerChairs[activePlayer.Id] = chair;
        }

        public void RemovePlayer(IPlayer player)
        {
            this.Players.Remove(player);
            player.CurrentTable = null;
        }

        public bool IsFull()
        {
            return this.Players.Count == MAX_NUMBER_OF_PLAYERS;
        }

        public bool HasPlayerJoinedTheTable(string playerId)
        {
            return this.Players.Any(x => x.Id == playerId);
        }

        #endregion

        #region ITableGameController

        /// <summary>
        /// Starts the game for this table
        /// </summary>
        public void StartNewGame()
        {
            this._dealer.StartNewGame(this.Players);
            this._currentRound = GameRound.BlockOneRoundOne;
        }

        /// <summary>
        /// Starts a new game round
        /// </summary>
        public void StartNewRound()
        {
            this.MoveToTheNextRound();
            this._dealer.StartNewRound(this._currentRound);
        }

        #endregion

        #region private methods
        
        /// <summary>
        /// Returns the player that matches the parameter value
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private IPlayer GetPlayerById(IPlayer player)
        {
            var ret =  this.Players.FirstOrDefault(x => x.Equals(player));
            
            if (ret == null)
            {
                throw new PlayerNotFoundException(String.Format("The player with the the Id {0} was not found", player.Id));
            }

            return ret;
        }

        /// <summary>
        /// Checks if the given chair is already taken
        /// by some other player
        /// </summary>
        /// <returns></returns>
        private bool IsChairOccupied(char chair)
        {
            return this.Players.Any(x => x.CurrentChair == chair);
        }

        /// <summary>
        /// Checks if the game can start, in which case notifies
        /// the game controller.
        /// </summary>
        protected virtual void OnGameCanStart()
        {
            if (this.GameCanStart != null)
            {
                IList<string> playerIds = this.Players.Select(player => player.Id).ToList();
                var args = new TableCanStartEventArgs(playerIds);

                this.GameCanStart(this, args);   
            }
        }

        /// <summary>
        /// Changes the current round to the next one
        /// </summary>
        private void MoveToTheNextRound()
        {
            this._currentRound += 1;
        }

        #endregion
    }
}