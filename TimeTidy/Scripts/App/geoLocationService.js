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