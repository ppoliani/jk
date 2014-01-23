

/**
 * Ac custom durandal modal that shows a message for creating a new table
 *
 * @module  durandal/modals/createTable/createTableMsgBox
 */
define(['plugins/dialog'],
    function (dialog) {

        //#region private fields


        //#endregion

        //#region private methods

        var selectOption = function (dialogResult) {
            dialog.close(this, dialogResult, this.tableName(), this.password());
        };

        //#endregion

        //#region constructor

        /**
         * Constructor
         *
         * @constructor
         */
        var CreateTableMsgBox = function () {
            /**The message displayed in this modal dialog*/
            this.message = 'Table Info';
            
            /**The title of this modal dialog*/
            this.title = 'Create New Table';
            
            /**Button options*/
            this.options = ['Cancel'];
            
            /**The name of the table*/
            this.tableName = ko.observable();

            /**The password for this table*/
            this.password = ko.observable();
            
            /**The class value of the ok  button*/
            this.okBtnCss = ko.computed(function() {
                return this.tableName() && this.tableName().length
                    ? ''
                    : 'disabled';
            }, this);
        };

        CreateTableMsgBox.prototype = function () {

            //#region public API

            return {
                constructor: CreateTableMsgBox,
                selectOption: selectOption
            };

            //#endregion

        }();

        //#endregion

        //#region reveal

        return CreateTableMsgBox;

        //#endregion
    });