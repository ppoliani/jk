

/**
 * A module containing functionality for browser 
 * features support
 *
 * @module  utils/feateDetector
 */
define(function () {
       
    /**
     * Adds additional test to the current version of modernizr
     */
    (function() {
        Modernizr.addTest('svgfilters', function() {
            var result = false;
            try {
                result = typeof SVGFEColorMatrixElement !== undefined &&
                    SVGFEColorMatrixElement.SVG_FECOLORMATRIX_TYPE_SATURATE == 2;
            } catch(e) {
            }
            
            return result;
        });

        Modernizr.addTest('svgforeignobject', function () {
            var result = false;
            
            try {
                result = typeof SVGForeignObjectElement !== 'undefined';
            } catch(e) {
            }

            return result;
        });
        
    })();
        

    //#region inner methods
    
    var
        /**
         * Examines if svg filters feature is 
         * supported by the current browser
         */
        isSvgFiltersSupported = function () {
            return Modernizr.svgfilters;
        },
        
        /**
         * checks if svg foreignObject feature is 
         * supported by the current browser
         */
        isSvgForeignObjectSupported = function() {
            return Modernizr.svgforeignobject;
        },
        
        /**
         * Checks if the device has a touch screen
         */
        isTouchDevice = function() {
            return Modernizr.touch;
        };

    //#endregion

    //#region reveal

    return {
        svg: {
            isSvgFiltersSupported: isSvgFiltersSupported,
            isSvgForeignObjectSupported: isSvgForeignObjectSupported     
        },
        device: {
            isTouchDevice: isTouchDevice
        }
    };

    //#endregion
});