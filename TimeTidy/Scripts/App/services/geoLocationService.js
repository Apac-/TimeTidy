var GeoLocationService = function () {
    // 10 mins
    const recheckTimeout = 600000;

    var getCurrentPosition = function (success, fail) {
        if (!navigator.geolocation) {
            fail("Geolocation is not supported by your browser")
        } else {
            navigator.geolocation.getCurrentPosition(success, fail);

            setInterval(function () {
                navigator.geolocation.getCurrentPosition(success, fail);
            }, recheckTimeout)
        };

    };

    return {
        getCurrentPosition: getCurrentPosition, 
    };

}();