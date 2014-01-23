
define(function () {

    var
        /**
        * Implements the self-invoking constructor pattern
        * to eneforce new i.e. users that forget to add the
        * new keyword invoking a constructor
        * 
        * @param {object} scope - The scope passed when the constructor is invoked
        * @param {Function} ctor - The constructor function that is called
        *
        * @returns {object} The newly created instance 
        */
        enforceNew = function(scope, ctor) {
            if (!(scope instanceof ctor)) {
                return new ctor();
            }

            return scope;
        },
        
        /**
         * Delays the execution of the given function by the specified
         * amount of time
         */
        delay = function(callback, delayTime) {
            window.setTimeout(callback, delayTime);
        },
        
        /**
         * Turns the property names of the given object to
         * camecase
         */
        turnToCamelCase = function(obj) {
            for (var prop in obj) {
                var camelCase = firstLetterToLowerCase(prop),
                    val = obj[prop];
                delete obj[prop];
                obj[camelCase] = val;

            }
        },
        
        /**
         * Turns the first letter of the given string to lower case
         *
         * @param {string} str - The string whose first letter is modified
         */
        firstLetterToLowerCase = function(str) {
            return str.charAt(0).toLowerCase() + str.slice(1);
        },
        
        /**
         * Turns the first letter of the given string to upper case
         *
         * @param {string} str - The string whose first letter is modified
         */
        capitaliseFirstLetter = function(str) {
            return str.charAt(0).toUpperCase() + str.slice(1);
        },
        
       /**
         * Fills tha given entities object with models
         * that are created applying the mapper on the 
         * list of dtos
         *
         * @param {array} dtos - The raw list of dtos fetched from the back-end
         * @param {object} mapper - The mappers that creates models from the dto list
         */
        dtosToModels = function (dtos, mapper) {
            if (Array.isArray(dtos)) {
                return dtos.reduce(function(memo, dto) {
                    memo.push(mapper.fromDto(dto));
                    return memo;
                }, []);
            } else {
                return mapper.fromDto(dtos);
            }
        },
        
        /**
         * Executes the given function in asyn manner
         */
        deferExecution = function(fn, context) {
            var def = Q.defer();
            fn.call(context, def);
            return def.promise;
        };


    return {
        enforceNew: enforceNew,
        delay: delay,
        firstLetterToLowerCase: firstLetterToLowerCase,
        capitaliseFirstLetter: capitaliseFirstLetter,
        turnToCamelCase: turnToCamelCase,
        dtosToModels: dtosToModels,
        deferExecution: deferExecution
    };
});