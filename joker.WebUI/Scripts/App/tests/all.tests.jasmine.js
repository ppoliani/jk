/// <reference path="libs/require.js" />
/// <reference path="libs/jasmine.js" />
/// <reference path="libs/jasmine.async.js" />
/// <reference path="../../Scripts/jquery-1.9.1.js" />
/// <reference path="../../Scripts/jquery.signalR-1.1.2.js" />


requirejs.config({
    // same as the applications main baseUrl
    baseUrl: '../',
    shim: {
        'ko': {
            exports: 'ko'
        },
        
        'Q': {
            exports: 'Q'
        },
        
        'amplify': {
            exports: 'amplify'
        }
    },
    paths: {
        'knockout': '../Scripts/knockout-3.0.0',
        'Q': '../Scripts/q',
        'amplify': '../scripts/amplify'
    },
    
    // set the mappings so that durandal nows where to get the required modules
    map: {
        '*': {
            'durandal': '../Scripts/durandal',
            'plugins': '../Scripts/durandal/plugins',
            'transitions': '../Scripts/durandal/transitions'
        }
    }
});

// A small trick to turn the Q and knockout AMD modules to simple global variables.
// The latest version of knockout and q are AMD modules, but the tests were written before
// that update; so we treat them as global variables. This is a way to turn the amd modules
// to global variables
define('amdToGlobals', ['Q', 'knockout'], function (q, knockout) {
    ko = knockout;
    Q = q;
});

require(['amdToGlobals']);

// load all the tests the last parameters refer to global modules (loaded once visible by all modules)
requirejs(['tests/core/gameControllerTests',
        'tests/models/playingTableTests',
        'tests/realTime/hubTests',
        'tests/viewmodels/homeTests'],
    function() { }
);
