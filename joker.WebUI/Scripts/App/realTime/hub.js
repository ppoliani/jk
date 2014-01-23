
/**
 * A module that containts the logic for comunicating with the back-end
 *
 * @module  infrastructure/hub
 */
define(['services/logger',
        'utils/pubsub',
        'utils/utils',
        'models/model.mapper'],
    function (logger, pubsub, utils, modelMapper) {
        
        //#region private fields


        //#endregion

        //#region private methods

        var
            /**
             * Starts a connection witht the server (signalR) hub
             */
            startConnection = function () {
                var self = this,
                    def = Q.defer();
                
                var connection = $.hubConnection();
                this.proxy = connection.createHubProxy('gameHub');

                connection.start()
                    .done(function () {   
                        connectionStarted.call(self);
                        def.resolve();
                    })
                    .fail(function() {
                        def.reject();
                    });

                defineMethodsOnClient.call(this);
                
                return def.promise;
            },

            /**
             * Fires when a connection to the server has been established
             */
            connectionStarted = function () {
            },

            /**
             * defines the methods on client which the server hub
             * will be able to call
             */
            defineMethodsOnClient = function () {
                this.proxy.on('gameCanStart', gameCanStart);
                this.proxy.on('gameStarted', gameStarted);
                this.proxy.on('newTableAdded', newTableAdded);
            },

            /**
             * A callback that fires when the back-end has informed the client
             * that the game can start
             */
            gameCanStart = function () {
                throw new Error('Not implemented');
            },

            /**
             * A callback that fires when the back-end has informed the client
             * that the game has begun
             */
            gameStarted = function () {
                throw new Error('Not implemented');
            },
            
            /**
             * A callback that fires when a new table was added
             *
             * @param {object} table - The table that was added
             */
            newTableAdded = function (table) {
                table = utils.dtosToModels(table, modelMapper.playingTable);
                pubsub.trigger({
                    topic: 'newTableAdded',
                    data: table
                });
            },
            
            /**
             * Returns all the curent active tables
             */
            getActiveTables = function () {
                var def = Q.defer();
                
                this.proxy.invoke('GetActiveTables')
                    .done(function (tables) {
                        tables = utils.dtosToModels(tables, modelMapper.playingTable);
                        def.resolve(tables);
                        logger.log('Active tables retrieved from the back-end', null, null, false);
                    })
                    .fail(function(err) {
                        def.reject();
                        logger.logError('Error while retrieving active tables from the back-end ' + err, null, null, false);
                    });

                return def.promise;
            },

            /**
             * Return true if the current player is laready 
             * connected to the game
             */
            isPlayerConnected = function() {
                var def = Q.defer();
                this.proxy.invoke('IsPlayerConnected')
                    .done(function(isConnected) {
                        def.resolve(isConnected);
                        logger.log('user is connected? ' + isConnected, null, null, false);
                    })
                    .fail(function(err) {
                        def.reject();
                        logger.logError('Error while checking if player is connected ' + err, null, null, false);
                    });

                return def.promise;
            },

            /**
             * Makes a request to connect to the game
             *
             * @param {string} name - The name of the player that wasnt to join the game
             */
            requestConnectionToTheGame = function (name) {
                var def = Q.defer();
                
                this.proxy.invoke('JoinGame', name)
                    .done(function () {
                        def.resolve();
                        logger.log('Player ' + name + ' joined the game', null, null, false);
                    })
                    .fail(function (err) {
                        def.reject();
                        logger.logError('Player could not join the game: ' + err, null, null, false);
                    });

                return def.promise;
            },

            /**
             * Makes a request for the creation of a new table
             *
             * @param {name} - The name of the table to be created
             * @param {password} - The password for the new table
             */
            requestNewTableCreation = function (name, password) {
                password = password || '';

                return utils.deferExecution(function(def) {
                    this.proxy.invoke('CreateTable', name, password)
                      .done(function (table) {
                          table = utils.dtosToModels(table, modelMapper.playingTable);
                          def.resolve(table);
                          logger.log('The table ' + name + ' was successfully created', null, null, false);
                      })
                      .fail(function () {
                          def.reject();
                          logger.logError('The table ' + name + ' could not be created', null, null, false);
                      });

                }, this);
            },
            
            /**
             * Informs the back-end hub that the player wants to 
             * join the given table
             *
             * @param {string} tableId - The id of the table to join
             * @param {string} password - The password enter by the user
             */
            joinTable = function (tableId, password) {
                return this.proxy.invoke('JoinTable', tableId, password)
                        .then(function() {
                            logger.log('Successfully joined the table: ' + tableId, null, null, false);
                        })
                        .fail(function(err) {
                            logger.logError('Error while trying to join the table: ' + err, null, null, false);
                        });
            },
            
            /**
             * checks if the given password is correct for the 
             * specified table
             *
             * @param {string} tableId - The id of the table whose password is being verified
             * @param {string} password - The password enter by the user
             */
            checkTablePassword = function (tableId, password) {
                var def = Q.defer();
                this.proxy.invoke('CheckTablePassword', tableId, password)
                    .done(function(result) {
                        def.resolve(result);
                        logger.log('The Password that was provided was: ' + result, null, null, false);
                    })
                    .fail(function (err) {
                        logger.logError('Error while verifying table password' + err, null, null, false);
                        def.reject();
                    });

                return def.promise;
            };

        //#endregion

        //#region constructor

        /**
         * Creates instances of the Hub class
         *
         * @constructor
         */
        var Hub = function () {
            this.proxy = null;
        };

        Hub.prototype = function () {

            //#region public API

            return {
                constructor: Hub,
                startConnection: startConnection,
                requestConnectionToTheGame: requestConnectionToTheGame,
                getActiveTables: getActiveTables,
                requestNewTableCreation: requestNewTableCreation,
                checkTablePassword: checkTablePassword,
                joinTable: joinTable,
                isPlayerConnected: isPlayerConnected
            };

            //#endregion

        }();

        //#endregion

        //#region reveal

        return Hub;

        //#endregion
    });