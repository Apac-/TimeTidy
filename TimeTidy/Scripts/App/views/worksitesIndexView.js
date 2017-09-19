"use strict";
var WorksitesIndexView = function () {
    var populateSitesTable = function (jsonData) {
        $('#sites').DataTable({
            data: jsonData,
            columns: [
                {
                    data: "name",
                    render: function (data, type, workSite) {
                        return "<button class='btn-link js-select' data-lat=" + workSite.lat + " data-lng=" + workSite.lng + " data-siteId=" + workSite.id + ">" + data + "</button>";
                    }
                },
                {
                    data: "streetAddress"
                }
            ]
        });
    };

    return {
        populateSitesTable: populateSitesTable,
    };
}();