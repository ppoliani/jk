define(['durandal/system', 'durandal/plugins/router', 'services/logger'],
    function (system, router, logger) {

        var shell = {
            activate: activate,
            router: router
        };
        
        return shell;

        //#region Internal Methods
        function activate() {
            return boot();
        }

        function boot() {
            log('App Loaded!', null, true);

            return router.map([
                { route: '', moduleId: 'viewmodels/home', title: 'Joker App' }
            ]).buildNavigationModel()
                .mapUnknownRoutes('error', 'not-found')
                .activate({ pushState: false });
        }

        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });