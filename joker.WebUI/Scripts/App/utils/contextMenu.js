

/**
 * Contains logic for the right click event that displays the context menu
 *
 * @module  utils/contextMenu
 */
define(['utils/pubsub'],
    function (pubsub) {
        
        //#region consts

        var
            DEFAULT = 'default',
            VISIT_MENU = 'visit';

        //#endregion

        //#region private fields

        var
            menuList = {},
            currentMenu;

        //#endregion
        
        //#region event callbacks
        
        var
            ganttChartContainerClicked = function (event) {
                setCurrentMenu(event);       
            };

        //#endregion

        //#region private methods

        var
            /**
             * Performs initialization tasks
             */
            init = function () {
                createDefaultContextMenu();
                createVisitContextMenu();
                $('#gantt-chart').contextMenu(getContextMenu, {
                    theme: 'xp'
                });

                pubsub.on({
                    topic: 'ganttChartContainer:rightclick',
                    callback: ganttChartContainerClicked
                });
            },

            /**
             * Returns the context menu according to the
             */
            getContextMenu = function() {
                return menuList[currentMenu];
            },

            /**
             * Creates the menu that will be displayed when user right-clicks
             * a visit 
             */
            createVisitContextMenu = function () {
                menuList[VISIT_MENU] = [
                    { 'View Or Edit Vessel Call Details...': optionSelected },
                    { 'View Or Edit Cargo Details...': optionSelected },
                    { 'View Berth Options...': optionSelected },
                    $.contextMenu.separator,
                    { 'List Data Issues...': optionSelected },
                    $.contextMenu.separator,
                    { 'Delete Vessel Call': optionSelected },
                    { 'Copy Vessel Call': optionSelected }
                ];
            },
            
            /**
             * Creates the default menu
             */
            createDefaultContextMenu = function() {
                menuList[DEFAULT] = [
                    { 'Default Option 1': optionSelected },
                    { 'Default Option 2': optionSelected },
                    { 'Default Option 3': optionSelected }
                
                ];
            },

            /**
             * A callback function that is triggered when a menu option is selected
             */
            optionSelected = function (menuItem, menu) {
                alert(menuItem.textContent);
            },
            
            /**
             * Finds the menu that should be displayes based on the
             * location of the right-click event
             *
             * @param {object} event - The event information
             */
            setCurrentMenu = function(event) {
                currentMenu = isTargetVessel(event.target)
                    ? VISIT_MENU
                    : DEFAULT;
            },

            /**
             * Examines if the target of a click event is a vessel (i.e. visit)
             *
             * @param {object} target - The target object
             */
            isTargetVessel = function (target) {
                return target.nodeName === 'li'
                    || containsVisitContainer($(target).parents('g'));
            },
            
            /**
             * Checks if the given list contains a dom element
             * that represent a visit container (i.e. class= 'visit-container')
             */
            containsVisitContainer = function(elements) {
                var results = $.Enumerable.From(elements)
                    .Where(function (x) { return x.className.baseVal === 'visit-container'; })
                    .ToArray();

                return results.length;
            };

        //#endregion

        //#region reveal

        return {
            init: init
        };

        //#endregion
    });