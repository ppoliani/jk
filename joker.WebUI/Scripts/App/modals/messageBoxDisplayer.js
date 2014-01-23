

/**
 * Displays various kinds of message boxes and modals
 *
 * @module  durandal/modals/messageBoxDisplayer
 */
define(['durandal/plugins/dialog',
        'modals/createTable/createTableMsgBox',
        'modals/joinTable/joinTableMsgBox'],
    function (modalDialog, createTableMsgBox, joinTableMsgBox) {

        //#region enum
        
        var MessageTypes = {
            CreateTable: 0,
            JoinTable: 1
        };

        //#endregion

        //#region private fields


        //#endregion

        //#region private methods

        var
            /**
             * Displays the message box that corresponds to the given type
             *
             * @param {enum} msgType - The type of the message to be displayed
             * @param {object} args - The arguments to be passed to the specific modal constructor
             */
            showMessage = function (msgType, args) {
                switch (msgType) {
                    case MessageTypes.CreateTable:
                        return showCreateTableMessageBox();
                    case MessageTypes.JoinTable:
                        return showJoinTableMessageBox(args);
                    default:
                }
            },
            
            /**
             * Displays the create table message box
             */
            showCreateTableMessageBox = function() {
                return modalDialog.show(new createTableMsgBox());
            },
            
            /**
             * Displays the join table message box
             *
             * @param {object} args - The arguments to be passed to the specific modal constructor
             */
            showJoinTableMessageBox = function(args) {
                return modalDialog.show(new joinTableMsgBox(args));
            };

        //#endregion

        //#region reveal singleton

        return {
            MessageTypes: MessageTypes,
            showMessage: showMessage
        };

        //#endregion
    });