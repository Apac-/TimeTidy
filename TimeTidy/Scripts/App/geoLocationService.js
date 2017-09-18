function GeoLocationService() {
    var getCurrentPosition = function (success, fail) {
        if (!navigator.geolocation) {
            fail("Geolocation is not supported by your browser")
        };

        navigator.geolocation.getCurrentPosition(success, fail);
    };

}