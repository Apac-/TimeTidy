
var worksitesIndexController = function (mapboxService, geoLocationService, viewControll) {
    let siteMap;
    let currentCenter;

    let mapLoaded = $.Deferred();
    let tableLoaded;

    var init = function () {
        setUpSiteMap(mapLoaded);

        geoLocationService.getCurrentPosition(success, fail);

        let tableLoaded = $.getJSON("/api/worksites", { get_param: 'value' });


        $.when(tableLoaded, mapLoaded).done(function () {
            viewControll.populateSitesTable(tableLoaded.responseJSON);
        });

    };

    var setUpSiteMap = function (deferObj) {
        siteMap = mapboxService.createSiteMap("mapid");

        siteMap.on('load', function (e) {
            currentCenter = e.target.getCenter();
            deferObj.resolve();
        });
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
}(MapboxService, GeoLocationService, worksitesIndexView);