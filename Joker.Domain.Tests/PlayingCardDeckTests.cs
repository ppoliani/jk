using System.Linq;
using Joker.Domain.Entities.BoardGame.DeckOfCards.PlayingCards;
using NUnit.Framework;

namespace DeckOfCards.Tests
{
    [TestFixture]
    public class PlayingCardDeckTests
    {
        [Test]
        public void InitializeDeck()
        {
            var jokerlessDeck = new PlayingCardDeck();
            var deck = new PlayingCardDeck(3);

            Assert.AreEqual(0, jokerlessDeck.Count(x => x.Identity.IsJoker));
            Assert.AreEqual(3, deck.Count(x => x.Identity.IsJoker));
        }

        [Test]
        public void CardObjectEqualityCanBeTested()
        {
            var deck = new PlayingCardDeck(3);
            var card = deck.Card("5C");

            Assert.AreEqual(card, deck.Card("5C"));
        }

        [Test]
        public void DeckForJokerShouldHaveThirtySixCards()
        {
            // Arrange
            var deck = new PlayingCardDeck(CardSuits.Default(), CardIdentities.JokerCardIdentities(), 2, true);

            // Assert
            Assert.AreEqual(36, deck.Count());
        }

        [Test]
        public void DeckForJokerShouldNotHaveSixClubsCard()
        {
            // Arrange
            var deck = new PlayingCardDeck(CardSuits.Default(), CardIdentities.JokerCardIdentities(), 2, true);

            // Assert
            Assert.IsFalse(deck.Any(x => x.Suit.Name == "C" && x.Identity.Name == "Six"));
        }

        [Test]
        public void DeckForJokerShouldNotHaveSixSpadesCard()
        {
            // Arrange
            var deck = new PlayingCardDeck(CardSuits.Default(), CardIdentities.JokerCardIdentities(), 2, true);

            // Assert
            Assert.IsFalse(deck.Any(x => x.Suit.Name == "S" && x.Identity.Name == "Six"));
        }

        //[TestMethod]
        //public void GenerateAllCards()
        //{
        //    var deck = new PlayingCardDeck(3);

        //    foreach (var suit in deck.Suits)
        //    {
        //        TestCard("A" + suit.Code, 1, suit, "Ace", false, false);

        //        foreach (var num in Enumerable.Range(2, 7))
        //            TestCard(num.ToString(), suit, num, suit, num.ToString(), false, false);

        //        TestCard("T" + suit.Code, 10, suit, "Ten", false, false);

        //        var faceNum = 11;

        //        foreach (var code in new List<string> {"Jack", "Queen", "King"})
        //            TestCard(code[0].ToString() + suit.Code, faceNum++, suit, code, true, false);
        //    }

        //    foreach (var num in Enumerable.Range(0, 3))
        //        TestCard(num + "J", num, CardSuit.Joker, num.ToString(), true, true);

        //    //TestCard(CardIdentifier.FaceDownCard(), -1, CardSuit.None, "None", false, false);
        //}

        //private static void TestCard(CardSuit cardSuit, CardIdentity cardIdentity, int number, CardSuit suit, string code, bool isFaceCard,
        //                             bool isJoker)
        //{
        //    var card = new PlayingCard(cardSuit, cardIdentity);

        //    Assert.AreEqual(card.Identity, number);
        //    Assert.AreEqual(card.Suit, suit);
        //    Assert.AreEqual(card.Identity.Name, code);
        //    Assert.AreEqual(card.Identity.IsFaceCard, isFaceCard);
        //    Assert.AreEqual(card.Identity.IsJoker, isJoker);

        //    Console.WriteLine(card.Description + " - OK");
        //}
    }
}