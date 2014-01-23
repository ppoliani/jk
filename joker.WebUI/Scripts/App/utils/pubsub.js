

/**
 * A module that acts as a pub/sub facade
 *
 * @module  utils/pubsub
 */
define(['durandal/app'],
    function (app) {

        //#region private fields


        //#endregion

        //#region private methods

        var
            /**
             * Creates a new subscription
             *
             * @param {object} opts - A list of various options
             */
            on = function (opts) {
                if (Array.isArray(opts)) {
                    opts.forEach(function(el) {
                        app.on(el.topic, el.callback, el.context);
                    });
                } else {
                    app.on(opts.topic, opts.callback, opts.context);
                }
            },

            /**
             * Removes a subscription
             * 
             * @param {object} opts - A list of various options
             */
            off = function (opts) {
                if (Array.isArray(opts)) {
                    opts.forEach(function (el) {
                        app.off(el.topic, el.callback);
                    });
                } else {
                    app.off(opts.topic, opts.callback);
                }
            },

            /**
             * Notifies the subscribers of a topic
             *
             * @param {object} opts - A list of various options
             */
            trigger = function (opts) {
                var args = [],
                    data = opts.data;
                
                if (Array.isArray(data)) {
                    args = args.concat(data);
                } else {
                    args.push(data);
                }

                args.unshift(opts.topic);
                
                app.trigger.apply(app, args);
            };

        //#endregion

        //#region reveal

        return {
            on: on,
            off: off,
            trigger: trigger
        };

        //#endregion
    });