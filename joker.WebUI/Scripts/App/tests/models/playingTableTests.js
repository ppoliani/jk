
define(['config',
        'models/playingTable'],
    function(config, playingTable) {
        describe('The playingTable module', function() {

            config.testMode(true);

            var table = null;;

            beforeEach(function() {
                table = new playingTable();
            });

            afterEach(function() {
                table = null;
            });

            describe('The addPlayer method', function () {
                it('Should add a player to the player list', function() {
                    // Arrange
                    var player = {
                        name: 'playerName'  
                    };
                    
                    // Act
                    table.addPlayer(player);

                    // Assert
                    expect(table.players[0].name).toEqual('playerName');
                });
            });
        });
    });