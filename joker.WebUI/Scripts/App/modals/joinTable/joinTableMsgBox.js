

/**
 * Ac custom durandal modal that shows a dialog message for joining a new table
 *
 * @module  durandal/modals/createTable/joinTableMsgBox
 */
define(['plugins/dialog'],
    function (dialog) {

        //#region private fields


        //#endregion

        //#region private methods

        var selectOption = function (dialogResult) {
            dialog.close(this, dialogResult, this.password());
        };

        //#endregion

        //#region constructor

        /**
         * Constructor
         *
         * @constructor
         */
        var JoinTableMsgBox = function (opts) {
            /**The message displayed in this modal dialog*/
            this.message = 'Join Table';
            
            /**The title of this modal dialog*/
            this.title = 'Join The' + opts.tableName + ' Table';
            
            /**Button options*/
            this.options = ['Cancel'];

            /**The password for this table*/
            this.password = ko.observable();
            
            /**The class value of the ok  button*/
            this.okBtnCss = ko.computed(function() {
                return this.password() && this.password().length
                    ? ''
                    : 'disabled';
            }, this);
        };

        JoinTableMsgBox.prototype = function () {

            //#region public API

            return {
                constructor: JoinTableMsgBox,
                selectOption: selectOption
            };

            //#endregion

        }();

        //#endregion

        //#region reveal

        return JoinTableMsgBox;

        //#endregion
    });