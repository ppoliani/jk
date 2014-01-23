
/**
 * A module representing the game's players
 *
 * @module  models/player
 */
define(function () {
    
    //#region private fields


    //#endregion

    //#region private methods


    //#endregion

    //#region constructor

    /**
	 * Creates instances of the player class
	 *
	 * @constructor
	 */
    var Player = function (name, id) {
        this.id = id;
        this.name = name;
    };

    Player.prototype = function() {

        //#region public API

        return {
            constructor: Player
        };

        //#endregion

    }();

    //#endregion

    //#region reveal

    return Player;

    //#endregion
});