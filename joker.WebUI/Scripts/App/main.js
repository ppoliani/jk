require.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions'
    },
    
    // set the mappings so that durandal knows where to get the required modules
    map: {
        '*': {
            'durandal': '../Scripts/durandal',
            'plugins': '../Scripts/durandal/plugins',
            'transitions': '../Scripts/durandal/transitions'
        }
    }
});

define('jquery', function () { return jQuery; });
define('knockout', function() { return ko; });

define(['durandal/app', 'durandal/viewLocator', 'durandal/system', 'durandal/plugins/router', 'services/logger'],
    function (app, viewLocator, system, router, logger) {
        app.title = 'Joker';
        
        // specify which plugins to install and their configutation
        app.configurePlugins({
            router: true,
            dialog: true,
            widget: {
                kinds: [''] // Todo how to register widgets?
            }
        });

        // Enable debug message to show in the console 
        system.debug(true);

        app.start().then(function () {
            toastr.options.positionClass = 'toast-bottom-right';
            toastr.options.backgroundpositionClass = 'toast-bottom-right';

            router.handleInvalidRoute = function (route, params) {
                logger.logError('No Route Found', route, 'main', true);
            };

            // When finding a viewmodel module, replace the viewmodel string 
            // with view to find it partner view.
            viewLocator.useConvention();
            
            //Show the app by setting the root view model for our application.
            app.setRoot('viewmodels/shell', 'entrance');
        });
});