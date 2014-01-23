
/**
 * A module containing the logic for controlling the game
 *
 * @module  core/gameController
 */
define(['utils/pubsub',
        'realTime/hub',
        'models/playingTable',
        'models/player'],
    function (pubsub, hub, PlayingTable, Player) {
        
        //#region private fields


        //#endregion
        
        //#region event callbacks

        var
            tableRequestClb = function (name, table) {
            this.activeTables.push(table);
        },            

            /**
             * Fires when someone else has added a new table
             */
            newTableAddedClb = function(table) {
                this.activeTables.push(table);
            },            

            connectionStartedClb = function () {
                
            };


        //#endregion

        //#region private methods

        var
            
            /**
             * starts a new connection to the server hub
             */
            startConnection = function() {
                return this.clientHub.startConnection();
            },
            
            /**
             * Checks if the player is already connected to the game
             */
            checkIfPlayerConnected = function () {
                var def = Q.defer();
                this.clientHub.isPlayerConnected()
                    .then(function (isConnected) {
                        if (isConnected) {
                            getActiveTables();
                        }
                        def.resolve(isConnected);
                    }.bind(this));

                return def.promise;
            },

            /**
             * Connects to the game
             *
             * @param {string} name - The name of the player that wants to connect
             */
            connectToTheGame = function (name) {
                var self = this,
                    def = this.clientHub.requestConnectionToTheGame(name);

                def.then(function () {
                    var playerId = self.clientHub.proxy.connection.id;
                    self.player = new Player(name, playerId);
                });
                
                return def;
            },

            /**
             * Creates a new playing table with the given name
             *
             * @param {string} name - The name of the table 
             * @param {password} - The password for the new table
             */
            createTable = function (name, password) {
                var self = this,
                    def = Q.defer();
                password = password || '';
                
                this.clientHub.requestNewTableCreation(name, password)
                    .then(function (table) {
                        tableRequestClb.call(self, name, table);
                        def.resolve();
                    })
                    .fail(function() {
                        def.reject();
                    });

                return def.promise;
            },

            /**
             * Processes the request of the given player to join
             * the given playing table
             *
             * @param {number} tableId - The id of the table that the player asked to join
             * @param {string} password - The password enter by the user
             */
            joinTable = function (tableId, password) {
                var self = this;
                return this.clientHub.joinTable(tableId, password)
                    .then(function () {
                        var table = self.findTable(tableId);
                        table.addPlayer(self.player);
                    })
                    .fail(function(error) {
                        console.log(error);
                    });
            },

            /**
             * Finds the given table in the list of all the
             * active tables in the game
             *
             * @param {number} tableId - The id of the table that is being searched
             */
            findTable = function (tableId) {
                var result = $.grep(this.activeTables(), function (item) {
                    return item.id === tableId;
                });

                if (result.length === 0) {
                    throw new Error('no matching table was found for this id');
                } else if (result.length === 1) {
                    return result[0];
                } else {
                    throw new Error('multiple tables found');
                }
            },
            
            /**
             * Gets the active tables
             */
            getActiveTables = function() {
                return this.clientHub.getActiveTables()
                           .then(function (tables) {
                               this.activeTables(tables);
                           }.bind(this));
            },
            
            /**
             * Returns the current connection id. Which in turn is the 
             * players id
             */
            getConnectionId = function () {
                return this.player.id;
            },
            
            /**
             * checks if the given password is correct for the 
             * specified table
             *
             * @param {string} tableId - The id of the table whose password is being verified
             * @param {string} password - The password enter by the user
             */
            checkTablePassword = function(tableId, password) {
                return this.clientHub.checkTablePassword(tableId, password);
            };

        //#endregion

        //#region constructor

        /**
         * Creates an instance of the gameController class
         *
         * @constructor
         */
        var GameController = function () {
            this.activeTables = ko.observableArray([]);
            this.clientHub = new hub();
            this.player = null;
            
            //#region events
            
            pubsub.on({
                topic: 'newTableAdded',
                callback: newTableAddedClb,
                context: this
            });

            //#endregion
        };

        GameController.prototype = function () {

            //#region public API

            return {
                constructor: GameController,
                startConnection: startConnection,
                connectToTheGame: connectToTheGame,
                createTable: createTable,
                joinTable: joinTable,
                findTable: findTable,
                checkTablePassword: checkTablePassword,
                getConnectionId: getConnectionId,
                checkIfPlayerConnected: checkIfPlayerConnected,
                getActiveTables: getActiveTables
            };

            //#endregion

        }();

        //#endregion

        //#region reveal

        return GameController;

        //#endregion
    });