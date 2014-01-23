
define(['utils/pubsub'], function (pubsub) {
    
    //#region private fields

    var
        _testMode = false;

    //#endregion
    
    //#region private methods
    
    var
        init = function () {
            if (_testMode) {
                // inform all the modules that app runs in test mode
                pubsub.trigger({
                    topic: 'testMode:on'
                });
            }
        },
        
        /**
         * Sets the application execution to test mode
         */
        testMode = function(val) {
            if (val) {
                _testMode = val;
            }

            init();
            return _testMode;
        };

    //#endregion
    
    //#region reveal singleton 
    
    return {
        debugMode: true,
        testMode: testMode
    };
    
    //#endregion
    
});