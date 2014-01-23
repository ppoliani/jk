using System;
using System.Collections.Generic;
using System.Linq;
using Joker.Domain.Entities.BoardGame;
using Joker.Domain.Entities.BoardGame.DeckOfCards;
using Joker.Domain.Entities.Interfaces;

namespace Joker.Domain.Entities
{
    /// <summary>
    /// Represents the players of the games
    /// </summary>
    public class Player : IPlayer
    {
        #region IPlayer implementation

        public string Id { get; set; }
        public string Name { get; set; }
        public IPlayingTable CurrentTable { get; set; }
        public char CurrentChair { get; set; }
        public ICardSet Hand { get; set; }

        public void ResetHand()
        {
            if (this.Hand.Cards != null)
            {
                this.Hand.Cards.Clear();
            }        
        }

        #endregion

        #region ctor

        public Player(string name)
        {
            this.Name = name;
            this.Hand = new Hand();
        }

        #endregion
    }
}