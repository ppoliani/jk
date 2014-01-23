using System;
using System.Collections.Generic;
using Joker.Domain.Entities.BoardGame.DeckOfCards.PlayingCards;

namespace Joker.Domain.Entities.BoardGame.DeckOfCards
{
    /// <summary>
    ///     Interface defining a card set
    /// </summary>
    public interface ICardSet
    {
        Guid CardSetId { get; }
        IList<ICard> Cards { get; set; }
        int ItemCount { get; }
    }
}