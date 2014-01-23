

/**
 * A module that contains methods for mapping
 * the raw dtos fetched from the back-end, to 
 * its in memory model counterparts
 *
 * @module  models/model.mapper
 */
define(['models/model'], function(model) {

    //#region private fields


    //#endregion

    //#region private methods

    var
        playingTable = {
            getDtoId: function (dto) {
                return dto.id;
            },

            /**
             * Maps the given dto to an instance of
             * playingTable model. 
             *
             * @param {obj} dto - The dto fetched from the back-end
             * @param {entity} entity - The model that the dto will be mapped to. 
             */
            fromDto: function (dto, entity) {
                entity = entity || new model.PlayingTable();
                entity.id = dto.id;
                entity.name = dto.name;
                entity.hasPassword = dto.hasPassword;

                return entity;
            }
        };

       

    //#endregion

    //#region reveal singleton

    return {
        playingTable: playingTable
    };

    //#endregion
});