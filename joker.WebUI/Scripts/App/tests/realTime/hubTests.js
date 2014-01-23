
define(['config',
        'realTime/hub'],
    function(config, hub) {
        describe('The hub module', function() {

            config.testMode(true);

            var clientHub = null;

            beforeEach(function() {
                clientHub = new hub();
            });

            afterEach(function() {
                clientHub = null;
            });

            describe('The requestConnectionToTheGame method', function () {
                it('Should call the invoke method of the server hub', function () {
                    // Arrange
                    var h = jasmine.createSpyObj('proxy', ['invoke']);
                    h.invoke.andReturn({ done: function () { return { fail: function () { } }; } });
                    clientHub.proxy = h;

                    // Act
                    clientHub.requestConnectionToTheGame();

                    // Assert
                    expect(h.invoke).toHaveBeenCalled();
                });
            });
        });
    });