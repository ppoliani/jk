using System;
using System.Collections.Generic;
using Joker.Domain.Entities.BoardGame;

namespace Joker.Domain.Entities.Interfaces
{
    public interface IPlayingTable
    {
        #region properties

        /// <summary>
        /// Tables unique indentifier
        /// </summary>
        string Id { get; set; }

        string Name { get; set; }

        /// <summary>
        /// Collection of users currently playing on this table
        /// </summary>
        IList<IPlayer> Players { get; set; }


        /// <summary>
        /// The information about each round
        /// </summary>
        IDictionary<GameRound, GameRoundInfo> GameRoundsInfo { get; set; }

        #endregion

        #region Events

        event EventHandler<TableCanStartEventArgs> GameCanStart;

        #endregion

        #region methods

        /// <summary>
        /// The scores of all players for all the played rounds
        /// </summary>
        IDictionary<IPlayer, IRoundScore> UserScores { get; set; }

        /// <summary>
        /// Add the specified player to the list of 
        /// players
        /// </summary>
        /// <param name="player"></param>
        void AddPlayer(IPlayer player);

        /// <summary>
        /// Checks if the given password matches the password
        /// of this table (if any specified)
        /// </summary>
        /// <param name="password"></param>
        bool Authenticate(string password);

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
        void PickChair(IPlayer player, char chair);

        /// <summary>
        /// removes the specified player from the  
        /// list of players
        /// </summary>
        /// <param name="player"></param>
        void RemovePlayer(IPlayer player);

        /// <summary>
        /// Returns true if the table is full
        /// and there is no free space for another
        /// player to join
        /// </summary>
        /// <returns></returns>
        bool IsFull();

        /// <summary>
        /// Returns true if the given player has already joined the table
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        bool HasPlayerJoinedTheTable(string playerId);

        #endregion

        
    }
}