
var WorksitesIndexController = function (mapboxService, geoLocationService, viewControll) {
    let siteMap;
    let currentCenter;

    var init = function () {
        let mapLoaded = setUpSiteMap(siteMap);

        let tableLoaded = $.getJSON("/api/worksites", { get_param: 'value' });

        geoLocationService.getCurrentPosition(success, fail);


        $.when(tableLoaded, mapLoaded).done(function () {
            viewControll.populateSitesTable(tableLoaded.responseJSON);

            mapboxService.addMarkersToMap(siteMap, tableLoaded.responseJSON, onMarkerClick);
        });

    };

    var onMarkerClick = function (e) {
        setSelectedSite(e.target.options.title, e.target.id);
    };

    var setSelectedSite = function (siteName, siteId) {
        viewControll.setSite(siteName);

        $.getJSON("/api/timesheets/" + siteId, function (jsonData) {
            
        });
    };

    var setUpSiteMap = function (siteMap) {
        let deferObj = $.Deferred;

        siteMap = mapboxService.createSiteMap("mapid");

        siteMap.on('load', function (e) {
            currentCenter = e.target.getCenter();
            deferObj.resolve();
        });

        return deferObj;
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
}(MapboxService, GeoLocationService, WorksitesIndexView);