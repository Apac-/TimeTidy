
var WorksitesIndexController = function (mapboxService, geoLocationService, userLoggingService, timeSheetsService, worksitesService, viewControll) {
    let siteMap;
    let currentCenter;

    let workSites;
    let selectedSite;
    let currentTimeSheetId

    let userLatitude;
    let userLongitude;

    var init = function () {
        selectedSite = null;
        currentTimeSheetId = null;

        let mapLoaded = setUpSiteMap();

        let tableLoaded = worksitesService.getWorksites();

        geoLocationService.getCurrentPosition(getPositionSuccess, getPositionFail);

        $('#sites').on('click', '.js-select', onSiteClick);
        $('#logbtns').on('click', '.js-logon', onLogonButtonClick);
        $('#logbtns').on('click', '.js-logoff', onLogoffButtonClick);

        $.when(tableLoaded, mapLoaded).done(function () {
            workSites = tableLoaded.responseJSON;

            viewControll.populateSitesTable(workSites);

            mapboxService.addMarkersToMap(siteMap, workSites, onMarkerClick);

            let closestSite = findClosestSite(workSites, currentCenter);
            if (closestSite) {
                setSelectedSite(closestSite);

                mapboxService.setMapView(siteMap, selectedSite.lat, selectedSite.lng);
            }
        });
    };

    var onSiteClick = function (e) {
        let button = $(e.target);

        mapboxService.setMapView(siteMap, button.attr('data-lat'), button.attr('data-lng'));
        setSelectedSite(getWorkSite(button.attr('data-siteId')));
    };

    var onLogonButtonClick = function (e) {
        viewControll.loggingButtonClicked(e.target)

        let site = getWorkSite($(e.target).attr('data-siteId'));

        userLoggingService.userLogonToSite(site, userLatitude, userLongitude, logSuccess, logFailure);
    };

    var onLogoffButtonClick = function (e) {
        viewControll.loggingButtonClicked(e.target)

        userLoggingService.userLogoffOfSite(currentTimeSheetId,
                                            userLatitude, userLongitude,
                                            logSuccess, logFailure);
    };

    var onMarkerClick = function (e) {
        setSelectedSite(getWorkSite(e.target.id));
        setSelectedTimeSheet(e.target.id)
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

    var getWorkSite = function(siteId){
        let matchedSite = null;

        $.each(workSites, function (index, element) {
            if (element.id == siteId) {
                matchedSite = element;
            }
        });

        return matchedSite;
    };

    var setSelectedTimeSheet = function (siteId) {
        let success = function (data) {
            currentTimeSheetId = data.timeSheetId;

            if (data.dateTime === null) {
                viewControll.setLogButton(false);
            } else {
                viewControll.setLogButton(true);
            };
        };

        let fail = function () {
            viewControll.reportError('Timesheet was not found for given: Try reloading the page')
        };

        timeSheetsService.getTimeSheet(siteId, success, fail);
    };

    var setSelectedSite = function (site) {
        viewControll.setSite(site.name);

        selectedSite = site;
    };

    var setUpSiteMap = function () {
        let deferObj = $.Deferred();

        siteMap = mapboxService.createSiteMap("mapid");

        siteMap.on('load', function (e) {
            currentCenter = e.target.getCenter();
            deferObj.resolve();
        });

        return deferObj.promise();
    };

    var logSuccess = function(){
        setTimeout(function () { location.reload(true); }, 2000);
    };

    var logFailure = function(sheetId){
        if (sheetId) {
            alert(`Failed to log off from site. Reload and try again. SheetID: ${sheetId}`)
        } else {
            alert("Failed to log on to site. Reload and try again.");
        };
    };

    var getPositionSuccess = function (position) {
        userLatitude = position.coords.latitude;
        userLongitude = position.coords.longitude;

        mapboxService.setMapView(siteMap, userLatitude, userLongitude);
    };

    var getPositionFail = function (message) {
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
}(MapboxService, GeoLocationService, UserLoggingService, TimeSheetsService, WorksitesService, WorksitesIndexView);