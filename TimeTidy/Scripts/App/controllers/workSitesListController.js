"use strict";

var WorksitesListController = function (mapboxService, worksitesService, geoLocationService, worksitesListView) {
    let siteMap;

    let mapLayerGroup;

    let table = null;

    var init = function () {
        siteMap = mapboxService.createSiteMap('mapid');

        mapLayerGroup = L.layerGroup().addTo(siteMap);

        worksitesService.getWorksites(sucess, fail);
    };

    var success = function (data) {
        if (data.length != 0) {
            mapboxService.setMapView(siteMap, data[0].lat, data[0].lng);
        } else {
            geoLocationService.getCurrentPosition(function (position) {
                mapboxService.setMapView(siteMap, position.coords.latitude, position.coords.longitude);
            });
        };

        table = worksitesListView.populateWorksitesTable(data);

        addSiteMarkersToMap(data);
    };

    var addSiteMarkersToMap = function (data) {
        $.each(data, function (index, element) {
                mapboxService.addMarkerToMap(mapLayerGroup,
                                             element.lat, element.lng,
                                             element.name, element.name,
                                             `<b>${element.name}</b><br>${element.streetAddress}`)
        });
    };

    var fail = function () {
        worksitesListView.reportError("Failed to retrive worksites from server. Try reloading.")
    };

    return {
        init: init,
    };

}(MapboxService, WorksitesService, GeoLocationService, WorksitesListView);