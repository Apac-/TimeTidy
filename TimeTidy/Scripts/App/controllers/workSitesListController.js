"use strict";

var WorksitesListController = function (mapboxService) {
    let siteMap;

    let mapLayerGroup;

    let table = null;

    var init = function () {
        siteMap = mapboxService.createSiteMap('mapid');

        mapLayerGroup = L.layerGroup().addTo(siteMap);

        loadSites();
    };

    var loadSites = function () {

    };

    var success = function (data) {

    };

    var fail = function () {

    };

    return {
        init: init,
    };

}(MapboxService);