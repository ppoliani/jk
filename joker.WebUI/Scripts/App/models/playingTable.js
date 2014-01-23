
/**
 * A module that reprents a playing table
 *
 * @module  models/playingTable
 */
define(function () {
    
    //#region private fields


    //#endregion

    //#region private methods

    var
        addPlayer = function (player) {
            this.players.push(player);
        };

    //#endregion

    //#region constructor

    /**
	 * Creates instances of the playing table class
	 *
	 * @constructor
	 */
    var PlayingTable = function(name) {
        this.id = null;
        this.players = [];
        this.name = name;
        this.hasPassword = undefined;
    };

    PlayingTable.prototype = function() {

        //#region public API

        return {
            constructor: PlayingTable,
            addPlayer: addPlayer
        };

        //#endregion

    }();

    //#endregion

    //#region reveal

    return PlayingTable;

    //#endregion
});