
var worksitesIndexController = function (mapboxService, geoLocationService) {
    var siteMap;

    var init = function () {
        siteMap = mapboxService.createSiteMap();
        geoLocationService.getCurrentPosition(success, fail);
    };

    var success = function () {
        let output = $("#out");

        let latitude = position.coords.latitude;
        let longitude = position.coords.longitude;

        output.attr("userLat", latitude);
        output.attr("userLng", longitude);
        siteMap.setView([latitude, longitude], 13);
    };

    var fail = function (message) {
        let output = $("#out");

        if (message) { output.append(`<p>${message}</p>`) }
        else {
           output.append("<p>Unable to retrieve your location</p>") 
           alert("Fail in geolocate");
        };
    };

    return {
        init: init
    };
}(MapboxService, GeoLocationService);