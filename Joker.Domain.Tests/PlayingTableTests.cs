using System;
using System.Linq;
using Joker.Domain.Entities;
using Joker.Domain.Entities.BoardGame;
using Joker.Domain.Entities.Interfaces;
using Joker.Domain.Exceptions;
using Moq;
using NUnit.Framework;

namespace Joker.Domain.Tests
{
    [TestFixture]
    public class PlayingTableTests
    {
        private PlayingTable _playingTable;
        private const string DUMMY_PLAYER_ID = "id1234";
        private const char DUMMY_CHAIR = 'N';

        [SetUp]
        public void SetUp()
        {
            this._playingTable = new PlayingTable(DUMMY_PLAYER_ID, "new table");
        }

        [TearDown]
        public void TearDown()
        {
            this._playingTable.Players.Clear();
        }

        protected IPlayer AddPlayer()
        {
            var player = new Player("player")
                {
                    Id = new Guid().ToString()
                };
            this._playingTable.AddPlayer(player);
            var addedPlayer = this._playingTable.Players[0];
            return addedPlayer;
        }

        [TestFixture]
        public class TheAddPlayerMethod : PlayingTableTests
        {
            [Test]
            [ExpectedException(typeof(TableIsFullException))]
            public void ThrowsAnException_WhenTheTableIsFull()
            {
                // Arrange
                this._playingTable.AddPlayer(new Player("player"));
                this._playingTable.AddPlayer(new Player("player"));
                this._playingTable.AddPlayer(new Player("player"));
                this._playingTable.AddPlayer(new Player("player"));

                // Act
                this._playingTable.AddPlayer(new Player("player"));
            }

            [Test]
            public void AddNewPlayer_WhenTheIsFreeSpace()
            {
                // Arrange
                var player = new Player("player");

                // Act
                this._playingTable.AddPlayer(player);
                var addedPlayer = this._playingTable.Players[0];

                // Assert
                Assert.AreSame(player, addedPlayer);
            }

            [Test]
            public void AddNewPlayerAndAssingTheCurrentTable_WhenThereIsFreeSpace()
            {
                // Arange 
                var mockedPlayer = new Mock<IPlayer>();
                mockedPlayer.SetupAllProperties();
                mockedPlayer.Setup(x => x.Name).Returns("player");

                // Act
                this._playingTable.AddPlayer(mockedPlayer.Object);
                var assignedTable = mockedPlayer.Object.CurrentTable;

                // Assert
                Assert.AreEqual(this._playingTable, assignedTable);
            }
        }

        [TestFixture]
        public class ThePickChairmethod : PlayingTableTests
        {
            [Test]
            public void AssignsTheGivenChaitToThePlayer()
            {
                // Arrange
                var player = AddPlayer();

                // Act
                _playingTable.PickChair(player, DUMMY_CHAIR);

                // Assert
                Assert.AreEqual(DUMMY_CHAIR, player.CurrentChair);
            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowException_WhenPlayerSitOnAnOccupiedChair()
            {
                // Arrange
                var player = AddPlayer();
                var player2 = AddPlayer();

                // Act
                _playingTable.PickChair(player, DUMMY_CHAIR);
                _playingTable.PickChair(player2, DUMMY_CHAIR);
            }
        }

        [TestFixture]
        public class TheRemovePlayerMethod : PlayingTableTests
        {
            [Test]
            public void RemovesThePlayerFromTheList()
            {
                // Arrange
                var player = new Player("player");
                this._playingTable.AddPlayer(player);

                // Act
                this._playingTable.RemovePlayer(player);
                var isInTheList = this._playingTable.Players.Contains(player);

                // Assert
                Assert.IsFalse(isInTheList);
            }
        }

        [TestFixture]
        public class TheHasPlayerJoinedTheTableMethod : PlayingTableTests
        {
            [Test]
            public void ReturnsFalseIfThePlayerHasNotPreviouslyJoinedTheTable()
            {
                // Act
                var result = this._playingTable.HasPlayerJoinedTheTable(DUMMY_PLAYER_ID);

                // Assert
                Assert.IsFalse(result);
            }

            [Test]
            public void ReturnsTrueIfThePlayerHasNotPreviouslyJoinedTheTable()
            {
                // Arrange
                var addedPlayer = AddPlayer();

                // Act
                var result = _playingTable.HasPlayerJoinedTheTable(addedPlayer.Id);

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestFixture]
        public class TheGameCanStartEvent : PlayingTableTests
        {
            private bool OnGameCanStartWasInvoked;

            [Test]
            public void IsNotTriggered_WhenTheMaximumAmountOfPlayersHasNotJoinedTheTable()
            {
                // Arrange
                _playingTable.GameCanStart += this.OnGameCanStart;

                // Act
                AddPlayer();

                // Assert
                Assert.IsFalse(OnGameCanStartWasInvoked);
            }

            [Test]
            public void IsTriggered_WhenTheMaximumAmountOfPlayersHasJoinedTheTable()
            {
                // Arrange
                _playingTable.GameCanStart += this.OnGameCanStart;
                AddPlayer();
                AddPlayer();
                AddPlayer();

                // Act
                AddPlayer();

                // Assert
                Assert.IsTrue(OnGameCanStartWasInvoked);
            }

            private void OnGameCanStart(object sender, TableCanStartEventArgs tableCanStartEventArgs)
            {
                this.OnGameCanStartWasInvoked = true;
            }
        }

        [TestFixture]
        public class TheStartNewRoundMethod : PlayingTableTests
        {
            [Test]
            public void ShouldChangeTheCurrentToTheNextOne()
            {
                // Act
                _playingTable.StartNewGame();
                _playingTable.StartNewRound();

                // Assert
                Assert.AreEqual(GameRound.BlockOneRoundTwo, _playingTable.CurrentRound);
            }

            [Test]
            public void ShouldAssignTwoCardToEachPlayer_InTheSecondRound()
            {
                // Arrange
                AddPlayer();
                AddPlayer();
                AddPlayer();
                AddPlayer();

                // Act
                _playingTable.StartNewGame();
                _playingTable.StartNewRound();

                // Assert
                var actual = _playingTable.Players.Count(x => x.Hand.Cards.Count() == 2);
                const int expected = 4;

                Assert.AreEqual(expected, actual);
            }
        }

        [TestFixture]
        public class TheAuthenticateMethod : PlayingTableTests
        {
            [Test]
            public void ShouldReturnFalse_WhenTheGivenPasswordIsWrong()
            {
                // Arrange
                var playingTable = new PlayingTable(DUMMY_PLAYER_ID, "Some Table Name", "1234");

                // Act
                var result = playingTable.Authenticate("123");

                // Assert
                Assert.IsFalse(result);
            }

            [Test]
            public void ShouldReturnTrue_WhenTheGivenPasswordIsWrong()
            {
                // Arrange
                var playingTable = new PlayingTable(DUMMY_PLAYER_ID, "Some Table Name", "1234");

                // Act
                var result = playingTable.Authenticate("1234");

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void ShouldReturnTrue_WhenTheGivenPasswordIsNullOrEmpty()
            {
                // Arrange
                var playingTable = new PlayingTable(DUMMY_PLAYER_ID, "Some Table Name");

                // Act
                var result = playingTable.Authenticate("");

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestFixture]
        public class TheHasPasswordProperty: PlayingTableTests
        {
            [Test]
            public void ShouldReturnFalse_WhenTheTableHasNotPassword()
            {
                // Arrange
                var playingTable = new PlayingTable(DUMMY_PLAYER_ID, "Some Table Name", "");

                // Act
                var result = playingTable.HasPassword;

                // Assert
                Assert.IsFalse(result);
            }

            [Test]
            public void ShouldReturnTrue_WhenTheTableHasPassword()
            {
                // Arrange
                var playingTable = new PlayingTable(DUMMY_PLAYER_ID, "Some Table Name", "1234");

                // Act
                var result = playingTable.HasPassword;

                // Assert
                Assert.IsTrue(result);
            }
        }

        [TestFixture]
        public class TheStartNewGameMethod : PlayingTableTests
        {
            // add test cases
        }
    }
}
