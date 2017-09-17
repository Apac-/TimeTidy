function setUpMap() {
    var siteMap = L.map("mapid");
    L
        .tileLayer("https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=ENTER_MAPBOX_ACCESS_KEY_HERE",
        {
            attribution:
                'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
                    '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
                    'Imagery © <a href="http://mapbox.com">Mapbox</a>',
            maxZoom: 18,
            id: "mapbox.streets"
        }).addTo(siteMap);

    return siteMap;
}
