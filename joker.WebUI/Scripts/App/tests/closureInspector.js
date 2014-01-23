

/**
 * Contains util methods that can get access to the closure variables
 * of any module that support the innerEval method
 *
 * @module  test/testUtils
 */
define(function() {

    //#region private fields


    //#endregion

    //#region private methods

    var
        /**
         * Gets the given closure member of the given module
         *
         * @param {object} module - An instance of the module whose closure is being accessed
         * @param {object} member - The member of the modules closure we're accessing
         */
        getClosureMember = function (module, member) {
            return module.innerEval(member);
        },
        
        /**
         * Sets the closure member of the module
         *
         * @param {object} module - An instance of the module whose closure is being accessed
         * @param {object} member - The member of the modules closure we're accessing
         * @param {object} value- The value to assign to the given member
         */
        setClosureMember = function (module, member, value) {
            // pass the value as a parameter to the innerEval so that it can be added to 
            // the variable object (scope) of that function. 
            module.innerEval(member + ' = arguments[1]', value);
        },
        
        /**
         * Replaces the given member of the given module's closure
         *
         * @param {object} module - An instance of the module whose closure is being accessed
         * @param {object} member - The member of the modules closure we're accessing
         * @param {object} replacement- The value to replace the given member
         */
        replaceClosureMember = function(module, member, replacement) {
            var orig = getClosureMember(module, member);
            setClosureMember(module, member, replacement);

            return orig;
        };

    //#endregion


    //#endregion

    //#region reveal

    return {
        setClosureMember: setClosureMember,
        getClosureMember: getClosureMember,
        replaceClosureMember: replaceClosureMember
    };

    //#endregion
});