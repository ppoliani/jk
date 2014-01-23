
/**
 * A modulel containing logic for speech recognition and command execution
 *
 * @module  utils/speech
 */
define(['durandal/app'],
    function (app) {

        //#region private fields

        var
            finalTranscript = '',
            interimTranscript = '',
            commands = {
                open: 'keypanel:fold',
                close: 'keypanel.unfold'
            };

        //#endregion

        //#region private methods

        var
            /**
             * Performs initialization tasks
             */
             init = function () {
                 initWebKitSpeechRecognition();
             },

             /**
              * Initializes the webkit web speech API
              */
            initWebKitSpeechRecognition = function () {
                if (('webkitSpeechRecognition' in window)) {
                    var recognition = new webkitSpeechRecognition();
                    recognition.continuous = true;
                    recognition.interimResults = true;

                    recognition.onresult = webKitSpeechApiResult;

                    recognition.start();
                }
            },

            /**
             * A callback trigered when the webkit api recognized a new transcript
             */
            webKitSpeechApiResult = function (event) {
                finalTranscript = '';
                for (var i = event.resultIndex; i < event.results.length; ++i) {
                    if (event.results[i].isFinal) {
                        finalTranscript += event.results[i][0].transcript;
                    } else {
                        interimTranscript += event.results[i][0].transcript;
                    }
                }
                finalTranscript.trim();
                if (finalTranscript in commands) {
                    app.trigger(commands[finalTranscript]);
                }
                console.log(finalTranscript);
            };

        //#endregion

        //#region reveal

        return {
            init: init
        };

        //#endregion
    });