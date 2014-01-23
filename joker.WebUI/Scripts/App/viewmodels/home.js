
/**
 * The home viewmodel
 *
 * @module  viewmodels/home
 */
define(['utils/pubsub',
        'services/logger',
        'realTime/hub',
        'core/gameController',
        'modals/messageBoxDisplayer'],
    function (pubsub, logger, hub, gameController, messageBoxDisplayer) {
        'use strict';
        
        //#region private fields


        //#endregion
        
        //#region event callback

        var
            /**
             * Callback function that is triggered when the user decides
             * to add a new table
             */
            addNewTable = function() {
                var self = this;

                messageBoxDisplayer.showMessage(messageBoxDisplayer.MessageTypes.CreateTable)
                    .then(function(btnClicked, tableName, password) {
                        if (btnClicked === 'Ok') {
                            self.gameController.createTable(tableName, password);
                        }
                    });
            },
            
            /**
             * Fires when the user clicks on a table entry
             * in the table of all active tables
             */
            tableSelected = function (obj, event) {
                var
                    // the reference of the home view model instance
                    vmContext = ko.contextFor(event.target).$parent,
                    tableId = this.id;
                
                if (!this.hasPassword) {              
                    vmContext.gameController.joinTable(tableId);
                    return;
                }
                
                messageBoxDisplayer.showMessage(messageBoxDisplayer.MessageTypes.JoinTable, {
                    tableName: this.name
                }).then(function(btnClicked, password) {
                    if (btnClicked === 'Ok') {
                        joinTable.call(vmContext, tableId, password);
                    }
                });
            },
            
            /**
             * Fires when the signalR connection has been established
             */
            connectionStartedClb = function () {
                this.gameController.checkIfPlayerConnected()
                    .then(function(isConnected) {
                        if (isConnected) {
                            this.isConnected(true);
                        }
                    }.bind(this));
            };
            
           

        //#endregion

        //#region private methods

        var
            activate = function () {
                logger.log('Home View Activated', null, 'home', true);
                return startConnection.call(this);
            },
            
            /**
             * Establishes the connection with the server hub
             */
            startConnection = function () {
                return this.gameController.startConnection()
                    .then(connectionStartedClb.bind(this));
            },
            
            /**
             * Connects to the game
             */
            connectToTheGame = function () {
                var self = this;
                
                this.gameController.connectToTheGame(this.userName())
                    .then(function() {
                        this.isConnected(true);
                        this.gameController.getActiveTables();
                    }.bind(this));
            },
            
            /**
             * Will try to add the current player to the table 
             * of his choice
             *
             * @param {string} tableId - The id of the table whose password is being verified
             * @param {string} password - The password enter by the user
             */
            joinTable = function (tableId, password) {
                this.gameController.joinTable(tableId, password)
                    .fail(function(err) {
                        // ToDo show a message that the user couldn join the given table
                    });
            };

        //#endregion

        //#region constructor

        /**
         * The home view model constructor
         *
         * @constructor
         */
        var Home = function () {
            this.gameController = new gameController();

            /**The user name of the player*/
            this.userName = ko.observable();
            
            /**The list of all the active tables*/
            this.activeTables = this.gameController.activeTables;
            
            /**The class value of the connect to the game button*/
            this.connectGameBtnClass = ko.computed(function() {
                return this.userName() && this.userName().length
                    ? ''
                    : 'disabled';
            }, this);
            
            /** Indicates if the user has connected to the game*/
            this.isConnected = ko.observable(false);
            
        };

        Home.prototype = function () {

            //#region public API

            var publicApi =  {
                constructor: Home,
                title: 'Tables',

                activate: activate,
                connectToTheGame: connectToTheGame,
                addNewTable: addNewTable,
                tableSelected: tableSelected
            };

            //#endregion
            
            //#region test api

            pubsub.on({
                topic: 'testMode:on',
                callback: function() {
                    $.extend(Home.prototype, {
                        startConnection: startConnection,
                        joinTable: joinTable
                    });
                }
            });
                    
            //#endregion

            return publicApi;

        }();

        //#endregion

        //#region reveal

        return Home;

        //#endregion
    });
