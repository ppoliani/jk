using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Joker.Domain.Entities.BoardGame;
using Joker.Domain.Entities.BoardGame.DeckOfCards;
using NUnit.Framework;

namespace Joker.Domain.Tests
{
    [TestFixture]
    public class DealerTests
    {
        private Dealer _dealer;

        [SetUp]
        public void testSetup()
        {
            this._dealer = new Dealer();
        }

        [TestFixture]
        public class TheAssignSetMethod : DealerTests
        {
            [Test]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsAnException_WhenTheCardSetIsNull()
            {
                // Act
                _dealer.AssignSet(GameRound.BlockOneRoundOne,  null);
            }

            [Test]
            public void AssingTheExpectedNumberOfCards()
            {
                // Arrange
                var hand = new Hand();

                // Act
                IEnumerable<ICard> cards = _dealer.AssignSet(GameRound.BlockOneRoundOne, hand);

                // Assert
                // 1 because the current round is the first (default)
                Assert.AreEqual(1, cards.Count());
            }
        }
    }
}