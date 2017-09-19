
var WorksitesIndexController = function (mapboxService, geoLocationService, viewControll) {
    let siteMap;
    let currentCenter;

    let workSites;
    let selectedSite;
    let currentTimeSheetId

    var init = function () {
        selectedSite = null;
        currentTimeSheetId = null;

        let mapLoaded = setUpSiteMap(siteMap);

        let tableLoaded = $.getJSON("/api/worksites", { get_param: 'value' });

        geoLocationService.getCurrentPosition(success, fail);

        $.when(tableLoaded, mapLoaded).done(function () {
            workSites = tableLoaded.responseJSON;

            viewControll.populateSitesTable(workSites);

            mapboxService.addMarkersToMap(siteMap, workSites, onMarkerClick);

            let closestSite = findClosestSite(workSites, currentCenter);
            setSelectedSite(closestSite);
        });
    };

    var findClosestSite = function (sites, currentPosition) {
        let shorestDistance = 0;
        let closeSite = null;
        let siteLoc;
        let dist;

        $.each(sites, function (index, element) {
            siteLoc = L.latLng(element.lat, element.lng);
            dist = currentPosition.distanceTo(siteLoc);
            if (shorestDistance == 0) {
                shorestDistance = dist;
                closeSite = element;
            } else {
                if (dist < shorestDistance) {
                    shorestDistance = dist;
                    closeSite = element;
                };
            };
        });

        return closeSite;
    };

    var onMarkerClick = function (e) {
        setSelectedSiteById(e.target.options.title, e.target.id);
        setSelectedTimeSheet(e.target.id)
    };

    var getWorkSite = function(sites, siteId){
        let matchedSite = null;

        $.each(sites, function (index, element) {
            if (element.id == siteId) {
                matchedSite = element;
            }
        });

        return matchedSite;
    };

    var setSelectedTimeSheet = function (siteId) {
        $.getJSON("/api/timesheets/" + siteId, function (jsonData) {
            currentTimeSheetId = jsonData.timeSheetId;

            if (jsonData.dateTime === null) {
                viewControll.setLogButton(false);
            } else {
                viewControll.setLogButton(true);
            };
        });
    };

    var setSelectedSite = function (site) {
        viewControll.setSite(site.name);

        selectedSite = site;
    };

    var setSelectedSiteById = function (siteName, siteId) {
        viewControll.setSite(siteName);

        selectedSite = getWorkSite(workSites, siteId);
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