"use strict";

var WorksitesListController = function (mapboxService, worksitesService, geoLocationService, viewControll) {
    let siteMap;

    let mapLayerGroup;

    let table = null;

    var init = function () {
        siteMap = mapboxService.createSiteMap('mapid');

        mapLayerGroup = L.layerGroup().addTo(siteMap);

        worksitesService.getWorksites(success, fail);

        $("#sites").on('click', '.js-delete', onDeleteClick);
    };

    var onDeleteClick = function () {
        let button = $(this);
        let siteName = button.attr('data-worksite-name');
        let siteId = button.attr('data-worksite-id')

        let deleteSuccess = function ($button, siteName) {
            viewControll.removeTableRowWithButton(table, $button);
            mapLayerGroup.eachLayer(function (layer) {
                if (layer.id == siteName) {
                    mapLayerGroup.removeLayer(layer);
                };
            });
        };

        bootbox.confirm({
            title: "Delete: " + siteName,
            message: "Are you sure you want to delete this work site?",
            callback: function (result) {
                if (result) {
                    worksitesService.deleteSite(siteId, deleteSuccess);
                }
            }
        });
    };

    var success = function (data) {
        if (data.length != 0) {
            mapboxService.setMapView(siteMap, data[0].lat, data[0].lng);
        } else {
            geoLocationService.getCurrentPosition(function (position) {
                mapboxService.setMapView(siteMap, position.coords.latitude, position.coords.longitude);
            });
        };

        table = viewControll.populateWorksitesTable(data);

        addSiteMarkersToMap(data);
    };

    var fail = function () {
        viewControll.reportError("Failed to retrive worksites from server. Try reloading.")
    };

    var addSiteMarkersToMap = function (data) {
        $.each(data, function (index, element) {
                mapboxService.addMarkerToMap(mapLayerGroup,
                                             element.lat, element.lng,
                                             element.name, element.name,
                                             `<b>${element.name}</b><br>${element.streetAddress}`)
        });
    };

    return {
        init: init,
    };

}(MapboxService, WorksitesService, GeoLocationService, WorksitesListView);