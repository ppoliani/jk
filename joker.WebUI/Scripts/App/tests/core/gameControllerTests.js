
define(['realTime/hub', 'core/gameController', 'models/player'],
    function (clientHub, gameController, player) {
        describe('The gameController module', function () {
            var controller,
                hub;

            // set up
            beforeEach(function () {
                controller = new gameController();
                hub = new clientHub();
                controller.clientHub = hub;
            });

            afterEach(function() {
                controller = null;
                hub = null;
            });
            
            var
                createSpyClientHub = function () {
                    var h = jasmine.createSpyObj('clientHub', ['requestNewTableCreation', 'joinTable']);
                    h.requestNewTableCreation.andReturn({
                        then: function (fn) {
                            fn({id: '123'});
                            return {
                                fail: function () {
                                }
                            };
                        }

                    });

                    h.joinTable.andReturn({
                        then: function(fn) {
                            fn();

                            return {
                                fail: function() {
                                }
                            };
                        }
                    });

                    return h;
                };
            
            describe('The CreateTable Method', function() {
                it('should call the requestNewTableCreation method of the hub class"', function () {
                    // Arrange
                    var h = createSpyClientHub();
                    controller.clientHub = h;

                    // Act
                    controller.createTable('new table');

                    // Assert
                    expect(h.requestNewTableCreation).toHaveBeenCalled();
                });
                
                it('should add the created table to the activeTables array"', function () {
                    // Arrange
                    controller.clientHub = createSpyClientHub();

                    // Act
                    controller.createTable('new table');

                    // Assert
                    expect(controller.activeTables()[0].id).toEqual('123');
                });

            });

            describe('The JoinTable Method', function() {
                var controller1,
                    hub1,
                    player1,
                    playingTable;

                // set up
                beforeEach(function() {
                    controller1 = new gameController();
                    hub1 = new clientHub();
                    player1 = new player('new player');
                    controller1.clientHub = hub1;
                });
                
                it('Should call the joinTable method of the clientHub', function () {
                    // Arrange
                    var h = jasmine.createSpyObj('clientHub', ['joinTable']);
                    h.joinTable.andReturn({
                        then: function () {
                            return {
                                fail: function() {
                                }
                            };
                        }
                    });
                    controller.clientHub = h;

                    // Act
                    controller.joinTable();

                    // Assert
                    expect(h.joinTable).toHaveBeenCalled();
                });

                it('throws an exception when the specified table is not in the list. This applies' +
                    ' when the request for joining to the table has returned from the back-end hub', function() {
                    // Arrange
                    controller1.clientHub = createSpyClientHub();
                    controller1.createTable('new table');
                    
                    var functionCall = function() {
                        controller1.joinTable(player1, '12');
                    };

                    // Assert
                    expect(functionCall).toThrow('no matching table was found for this id');
                });

                // should not thow any exception. The player will be added 
                // by an dependent unit, which tested elsewere
                it('adds the player to the specified table This applies' +
                    ' when the request for joining to the table has returned from the back-end hub', function() {
                    // Arrange
                    controller1.clientHub = createSpyClientHub();
                    
                    controller1.createTable('new table');
                    var createdTable = controller1.activeTables()[0];
                    createdTable.addPlayer = function() { };
                    spyOn(createdTable, 'addPlayer').andReturn();

                    var functionCall2 = function() {
                        controller1.joinTable(player1, '1234');
                    };
                });
            });
            
            describe('The connectToTheGame method', function () {
                it('Should call the requestConnectionToTheGame method of the clientHub', function () {
                    // Arrange
                    var h = jasmine.createSpyObj('clientHub', ['requestConnectionToTheGame']);
                    h.requestConnectionToTheGame.andReturn({
                        then: function () {}
                    });
                    controller.clientHub = h;

                    // Act
                    controller.connectToTheGame('name');

                    // Assert
                    expect(h.requestConnectionToTheGame).toHaveBeenCalled();
                });
            });

            describe('The startConnection method', function () {
                it('Should call the startConnection method of the clientHub', function () {
                    // Arrange
                    var h = jasmine.createSpyObj('clientHub', ['startConnection']);
                    h.startConnection.andReturn({
                        then: function () { }
                    });
                    controller.clientHub = h;

                    // Act
                    controller.startConnection();
                    
                    // Assert
                    expect(h.startConnection).toHaveBeenCalled();
                });
            });
            
            describe('The findTable method', function () {
                it('Should throw an exception when the table is not found', function () {
                    // Arrange
                    controller.activeTables = function () { return [{ id: 1 }]; };

                    // Assert
                    expect(function () { controller.findTable(2); }).toThrow(new Error('no matching table was found for this id'));
                });
                
                it('Should return the table when the one was found', function () {
                    // Arrange
                    controller.activeTables = function () { return [{ id: 1, name: 'tableName' }]; };

                    // Act
                    var result = controller.findTable(1);

                    // Assert
                    expect(result.name).toEqual('tableName');
                });
                
                it('Should throw an exception when multiple table are found', function () {
                    // Arrange
                    controller.activeTables = function () { return [{ id: 1 }, { id: 1 }]; };

                    // Assert
                    expect(function () { controller.findTable(1); }).toThrow(new Error('multiple tables found'));
                });
            });
            
            describe('The checkTablePassword method', function () {
                it('Should call the checkTablePassword method of the clientHub', function () {
                    // Arrange
                    var h = jasmine.createSpyObj('clientHub', ['checkTablePassword']);
                    h.checkTablePassword.andReturn({
                        then: function () { }
                    });
                    controller.clientHub = h;

                    // Act
                    controller.checkTablePassword();

                    // Assert
                    expect(h.checkTablePassword).toHaveBeenCalled();
                });
            });
        });
    });

