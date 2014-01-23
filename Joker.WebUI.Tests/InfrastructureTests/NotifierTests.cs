using Joker.Domain.Entities;
using Joker.Domain.Entities.Interfaces;
using Joker.WebUI.Infrastructure;
using Joker.WebUI.Infrastructure.Hubs;
using Joker.WebUI.Infrastructure.Interfaces;
using Moq;
using NUnit.Framework;

namespace Joker.WebUI.Tests.InfrastructureTests
{
    [TestFixture]
    public class NotifierTests
    {
        private GameController _gameController;
        private static string DUMMY_PLAYER_ID = "id1234";
        private static string DUMMY_TABLE_ID = "id4321";
        private static string DUMMY_TABLE_NAME = "tableName";
        private static string DUMMY_PLAYER_NAME = "player";

        // fakes
        private Mock<INotifier> _mockedGameInitializationNotifier;

        [SetUp]
        public void TestSetup()
        {
            this._gameController = GameController.Instance;
            this._mockedGameInitializationNotifier =new Mock<INotifier>();
            this._gameController.Notifier = _mockedGameInitializationNotifier.Object;
        }

        [TearDown]
        public void TearDown()
        {
            this._gameController.ActiveTables.Clear();
            this._gameController.GamePlayers.Clear();
        }

        protected IPlayingTable CreateTable(out string tableId)
        {
            this._gameController.CreateTable(DUMMY_PLAYER_ID, "new table");
            var newTable = this._gameController.ActiveTables[0];
            tableId = newTable.Id;
            return newTable;
        }

        private void AddEnoughPlayersToStartTheGame(string tableId)
        {
            var numOfRequiredPlayers = PlayingTable.MAX_NUMBER_OF_PLAYERS;
            
            for (int i = 0; i < numOfRequiredPlayers; i++)
            {
                var playerId = DUMMY_PLAYER_ID + "-" + i;
                var playerName = DUMMY_PLAYER_NAME + "-" + i;

                this._gameController.JoinGame(playerName, playerId);
                this._gameController.JoinTable(playerId, tableId);
            }
        }

        [TestFixture]
        public class TheInstanceProperty
        {
            [Test]
            public void CreatesAtLeastOneInstanceOfTheClass()
            {
                // Act
                var notifierSingl = Notifier.Instance;

                // Assert
                Assert.IsNotNull(notifierSingl);
            }

            [Test]
            public void DoesNotCreateAnInstance_WhenThereIsAlreadyOne()
            {
                // Act
                var notifierInst = Notifier.Instance;
                var notifierInst2 = Notifier.Instance;

                // Assert 
                Assert.AreSame(notifierInst, notifierInst2);
            }
        }

        [TestFixture]
        public class TheNotifyClientThatGameCanStartMethod : NotifierTests
        {
            [Test]
            public void IsNotInvoked_IfTheRequiredNumberOfPlayersHasNotJoinedTheTable()
            {
                // Arrange
                string tableId;
                var newTable = this.CreateTable(out tableId);
                _gameController.JoinGame(DUMMY_PLAYER_NAME, DUMMY_PLAYER_ID);
                
                // Act
                _gameController.JoinTable(DUMMY_PLAYER_ID, tableId);

                // Assert
                this._mockedGameInitializationNotifier.Verify(x => x.NotifyClientThatGameCanStart(DUMMY_PLAYER_ID), Times.Never());
            }

            [Test]
            public void IsInvoked_IfTheRequiredNumberOfPlayersHasJoinedTheTable()
            {
                // Arrange
                string tableId;
                this.CreateTable(out tableId);

                // Act
                this.AddEnoughPlayersToStartTheGame(tableId);

                // Assert
                this._mockedGameInitializationNotifier.Verify(x => x.NotifyClientThatGameCanStart(DUMMY_PLAYER_ID + "-0"), Times.Once());
            }
        }


        [TestFixture]
        public class TheNotifyClientThatGameHasStartedMethod : NotifierTests
        {
            [Test]
            public void IsInvoked_WhenAnyOfTablePlayerStartsTheGame()
            {
                // Arrange
                string tableId;
                CreateTable(out tableId);
                _gameController.JoinGame(DUMMY_PLAYER_NAME, DUMMY_PLAYER_ID);
                _gameController.JoinTable(DUMMY_PLAYER_ID, tableId);

                // Act
                _gameController.StartGameForTable(tableId);

                // Assert
                this._mockedGameInitializationNotifier.Verify(x => x.NotifyClientThatGameHasStarted(DUMMY_PLAYER_ID), Times.Once());
            }
        }
    }
}