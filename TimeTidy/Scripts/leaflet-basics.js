function geoFindMe(map) {
    var output = $("#out");

    if (!navigator.geolocation) {
        output.append("<p>Geolocation is not supported by your browser</p>");
        return;
    }

    function success(position) {
        var latitude = position.coords.latitude;
        var longitude = position.coords.longitude;

        output.attr("userLat", latitude);
        output.attr("userLng", longitude);
        map.setView([latitude, longitude], 13);
    }

    function error() {
        output.append("<p>Unable to retrieve your location</p>");
        alert("Fail in geolocat");
    }

    navigator.geolocation.getCurrentPosition(success, error);
}

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
