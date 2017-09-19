var MapboxService = function (mapboxApiToken) {
    var createSiteMap = function (mapRef) {
        var siteMap = L.map(mapRef);
        L
            .tileLayer("https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?" + mapboxApiToken.token,
            {
                attribution:
                    'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
                        '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
                        'Imagery © <a href="http://mapbox.com">Mapbox</a>',
                maxZoom: 18,
                id: "mapbox.streets"
            }).addTo(siteMap);

        return siteMap;
    };

    var addMarkerToMap = function (mapLayerGroup, lat, lng, title, id, message, clickEvent) {
        let marker

        if (clickEvent) {
            marker = L.marker([lat, lng], { title: title }).bindPopup(message).on('click', clickEvent);
        }
        else {
            marker = L.marker([lat, lng], { title: title }).bindPopup(message);
        };

        marker.id = id;
        mapLayerGroup.addLayer(marker);
    };

    return {
        createSiteMap: createSiteMap,
        addMarkerToMap: addMarkerToMap,
    };
}(MapboxApiToken);
