using System;
using System.Linq;
using Joker.Domain.Entities.Interfaces;
using Joker.Domain.Exceptions;
using Joker.WebUI.Infrastructure;
using Moq;
using NUnit.Framework;

namespace Joker.WebUI.Tests.InfrastructureTests
{
    [TestFixture]
    public class GameControllerTests
    {
        private GameController _gameController;
        private Mock<IPlayer> _mockedPlayer;
        private Mock<IPlayingTable> _mockedTable;
        private static string DUMMY_PLAYER_ID = "id1234";
        private static string DUMMY_TABLE_PASSWORD = "id1234";
        private static string DUMMY_PLAYER_NAME = "player";
        private static char DUMMY_CHAIR_NUM = 'N';

        [SetUp]
        public void TestSetup()
        {
            this._gameController = GameController.Instance;
            this._mockedPlayer = new Mock<IPlayer>();
            this._mockedTable = new Mock<IPlayingTable>();
        }

        [TearDown]
        public void TearDown()
        {
            this._gameController.ActiveTables.Clear();
            this._gameController.GamePlayers.Clear();
        }

        protected IPlayingTable CreateTable(out string tableId, string password = null)
        {
            this._gameController.CreateTable(DUMMY_PLAYER_ID, "new table", password);
            var newTable = this._gameController.ActiveTables [0];
            tableId = newTable.Id;
            return newTable;
        }

        [TestFixture]
        public class TheInstanceProperty
        {
            [Test]
            public void CreatesAtLeastOneInstanceOfTheClass()
            {
                // Act
                var gameControllerSingl = GameController.Instance;

                // Assert
                Assert.IsNotNull(gameControllerSingl);
            }

            [Test]
            public void DoesNotCreateAnInstance_WhenThereIsAlreadyOne()
            {
                // Act
                var gameControllerInst = GameController.Instance;
                var gameControllerInst2 = GameController.Instance;

                // Assert 
                Assert.AreSame(gameControllerInst, gameControllerInst2);
            }
        }

        [TestFixture]
        public class TheJoinGameMethod : GameControllerTests
        {
            [Test]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowAnException_WhenNameIsNullOrEmpty()
            {
                // Act
                this._gameController.JoinGame("", DUMMY_PLAYER_ID);

            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowAnException_WhenPlayerIdIsNullOrEmpty()
            {
                // Act
                this._gameController.JoinGame("","");

            }

            [Test]
            public void AddNewPlayer_WhenNameProvided()
            {
                // Arrange
                var name = "new player";
                
                // Act
                this._gameController.JoinGame(name, DUMMY_PLAYER_ID);
                var addedPlayer = this._gameController.GamePlayers[0];

                // Assert
                Assert.AreEqual(name, addedPlayer.Name);
            }

            [Test]
            public void AssignAnIdToTheUser_WhenHeJoindTheGame()
            {
                // Act
                this._gameController.JoinGame( "player name", DUMMY_PLAYER_ID);
                var addedPlayer = this._gameController.GamePlayers[0];

                // Assert
                Assert.IsNotNull( addedPlayer.Id );
            }
        }

        [TestFixture]
        public class TheCreateTableMethod : GameControllerTests
        {
            [Test]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsAnException_WhenAnEmptyNameIsPassed()
            {
                // Act
                this._gameController.CreateTable(DUMMY_PLAYER_ID, String.Empty);
            }

            [Test]
            public void ThrowsAnException_WhenActiveTablesListNotInitialized()
            {
                // Act
                this._gameController.CreateTable(DUMMY_PLAYER_ID, "new table");
            }

            [Test]
            public void CreatesANewTableCalledNewTable()
            {
                // Arrange 
                const string tableName = "new table";
                
                // Act
                this._gameController.CreateTable(DUMMY_PLAYER_ID, tableName);

                // Assert
                Assert.AreEqual(tableName, this._gameController.ActiveTables[0].Name);
            }

            [Test]
            public void CreateATableWithUniqueId()
            {
                // Act
                this._gameController.CreateTable(DUMMY_PLAYER_ID, "table1");
                this._gameController.CreateTable(DUMMY_PLAYER_ID, "table2");

                // Assert
                var table1Id = this._gameController.ActiveTables[0].Id;
                var table2Id = this._gameController.ActiveTables[1].Id;

                Assert.AreNotEqual(table1Id, table2Id);
            }
        }

        [TestFixture]
        public class TheJoinTableMethod : GameControllerTests
        {
            [Test]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsAnException_WhenPlayerIsNull()
            {
                // Arrange
                this._mockedTable.SetupAllProperties();

                // Act
                this._gameController.JoinTable(null, "tableId");
            }

            [Test]
            [ExpectedException(typeof(TableNotFoundException))]
            public void ThrowsAnException_WhenTheSpecifiedTableIsNotInTheListOfTheActiveTables()
            {
                // Act
                this._gameController.JoinTable("playerId", "tableId");
            }

            [Test]
            [ExpectedException(typeof(PlayerHasAlreadyJoinedTableException))]
            public void ThrowAnException_WhenTheSpecifiedPlayerHasAlreadyBeenAdded()
            {
                // Arrange
                this._gameController.JoinGame(DUMMY_PLAYER_NAME, DUMMY_PLAYER_ID);
                string tableId;
                var newTable = CreateTable(out tableId);
                this._gameController.JoinTable(DUMMY_PLAYER_ID, tableId);

                // Act
                this._gameController.JoinTable(DUMMY_PLAYER_ID, tableId);
            }

            [Test]
            [ExpectedException(typeof (WrongTablePasswordEntered))]
            public void ThrowsAnException_WhenTheSpecifiedPlayerEnteredTheWrongTablePassword()
            {
                // Arrange
                this._gameController.JoinGame(DUMMY_PLAYER_NAME, DUMMY_PLAYER_ID);
                string tableId;
                var newTable = CreateTable(out tableId, DUMMY_TABLE_PASSWORD);

                // Act
                this._gameController.JoinTable(DUMMY_PLAYER_ID, tableId, "wrong password!!");
            }

            [Test]
            public void AddThePlayerToTheSpecifiedTable()
            {
                // Arrange
                this._gameController.JoinGame("player1", DUMMY_PLAYER_ID);
                this._mockedPlayer.SetupAllProperties();
                this._mockedPlayer.Setup(x => x.Id).Returns(DUMMY_PLAYER_ID);

                string tableId;
                var newTable = CreateTable(out tableId);

                // Act
                this._gameController.JoinTable(DUMMY_PLAYER_ID, tableId);

                // Assert
                Assert.IsTrue(newTable.Players.Count(x => x.Id == DUMMY_PLAYER_ID) > 0);
            }

        }

        [TestFixture]
        public class ThePickChairMethod : GameControllerTests
        {
            [Test]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowException_WhenThePlayerHasNotJoinedAnyTable()
            {
                // Arrange
                this._gameController.JoinGame(DUMMY_PLAYER_NAME, DUMMY_PLAYER_ID);

                // Act
                this._gameController.PickChair(DUMMY_PLAYER_ID, DUMMY_CHAIR_NUM);
            }
        }

        [TestFixture]
        public class TheLeaveTableMethod : GameControllerTests
        {
            [Test]
            public void RemovesThePlayerFromTheSpecifiedTable()
            {
                // Arrange
                this._gameController.JoinGame("player1", DUMMY_PLAYER_ID);
                this._mockedPlayer.SetupAllProperties();
                this._mockedPlayer.Setup(x => x.Id).Returns(DUMMY_PLAYER_ID);

                string tableId;
                var newTable = CreateTable( out tableId );
                this._gameController.JoinTable(DUMMY_PLAYER_ID, tableId);
                this._mockedPlayer.Setup( x => x.CurrentTable ).Returns(newTable);

                // Act
                this._gameController.LeaveTable( DUMMY_PLAYER_ID );

                // Assert
                Assert.IsTrue(newTable.Players.Count(x => x.Id == DUMMY_PLAYER_ID) == 0);

            }
        }

        [TestFixture]
        public class TheLeaveGame : GameControllerTests
        {
            [Test]
            public void RemovesThePlayerFromTheSpecifiedTable()
            {
                // Arrange
                this._gameController.JoinGame("player1", DUMMY_PLAYER_ID);
                this._mockedPlayer.SetupAllProperties();
                this._mockedPlayer.Setup(x => x.Id).Returns(DUMMY_PLAYER_ID);

                // Act
                this._gameController.LeaveGame(DUMMY_PLAYER_ID);

                // Assert
                Assert.IsTrue(this._gameController.GamePlayers.Count(x => x.Id == DUMMY_PLAYER_ID) == 0);

            }
        }

        [TestFixture]
        public class TheGetTableByIdMethod : GameControllerTests
        {
            [Test]
            [ExpectedException(typeof(TableNotFoundException))]
            public void ThrowAnException_WhenTheSpecifiedTableDoesNotExist()
            {
                // arrange
                this._gameController.GetTableById("tableId");
            } 

            [Test]
            public void ReturnsTable_WhenThereIsOneWithTheSpecifiedId()
            {
                // Arrange
                this._gameController.CreateTable(DUMMY_PLAYER_ID, "new table");
                var newTable = this._gameController.ActiveTables[0];
                var tableId = newTable.Id;

                // Act
                var result = this._gameController.GetTableById(tableId);

                // Assert
                Assert.AreSame(newTable, result);
            }
        }

        [TestFixture]
        public class TheGetPlayerByIdMethod : GameControllerTests
        {
            [Test]
            [ExpectedException(typeof (PlayerNotFoundException))]
            public void ThrowsException_WhenPlayerIsNotFound()
            {
                // Act
                this._gameController.GetPlayerById(DUMMY_PLAYER_ID);
            }


            [Test]
            public void ReturnsPlayer_WhenThereIsOneWithTheSpecifiedId()
            {
                // Arrange 
                this._gameController.JoinGame(DUMMY_PLAYER_NAME, DUMMY_PLAYER_ID);

                // Act
                var player = this._gameController.GetPlayerById(DUMMY_PLAYER_ID);

                // Assert
                Assert.AreEqual(DUMMY_PLAYER_ID, player.Id);
            }
        }

        [TestFixture]
        public class TheCheckTablePasswordMethod: GameControllerTests
        {
            [Test]
            public void ShouldReturnFalse_WhenThePasswordDoesnMatch()
            {
                // Arrange
                this._gameController.CreateTable(DUMMY_PLAYER_ID, "new table", "1234");
                var newTable = this._gameController.ActiveTables[0];
                var tableId = newTable.Id;

                // Act
                var result = this._gameController.CheckTablePassword(tableId, "000");

                // Assert
                Assert.IsFalse(result);
            }

            [Test]
            public void ShouldReturnTrue_WhenThePasswordMatches()
            {
                // Arrange
                this._gameController.CreateTable(DUMMY_PLAYER_ID, "new table", "1234");
                var newTable = this._gameController.ActiveTables[0];
                var tableId = newTable.Id;

                // Act
                var result = this._gameController.CheckTablePassword(tableId, "1234");

                // Assert
                Assert.IsTrue(result);
            }
        }
    }
}
