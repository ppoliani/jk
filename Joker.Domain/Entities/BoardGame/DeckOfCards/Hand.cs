using System;
using System.Collections.Generic;
using Joker.Domain.Entities.BoardGame.DeckOfCards.PlayingCards;

namespace Joker.Domain.Entities.BoardGame.DeckOfCards
{
    public class Hand : ICardSet
    {
        private Guid _setId;

        #region properties

        public IList<ICard> Cards { get; set; }

        #endregion

        #region ICardSet Members

        public Guid CardSetId
        {
            get
            {
                if (_setId == Guid.Empty) _setId = Guid.NewGuid();
                return _setId;
            }
        }

        public int ItemCount
        {
            get { throw new NotImplementedException(); }
        }

        #endregion  
    }
}