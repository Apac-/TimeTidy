"use strict";

var WorksitesListView = function () {
    var populateWorksitesTable = function (data) {
        let table = $('#sites').DataTable({
            data: data,
            columns: [
                {
                    data: "name",
                    render: function(data, type, worksite) {
                        return "<a href='/worksites/edit/" + worksite.id + "'>" + worksite.name + "</a>";
                    }
                },
                {
                    data: "streetAddress"
                },
                {
                    data: "id",
                    render: function(data, type, worksite) {
                        return "<button class='btn-link js-delete' data-worksite-id=" + data 
                            + " data-worksite-name='" + worksite.name + "'>Delete</button>";
                    }
                }
            ]
        });

        return table;
    };

    var reportError = function (message) {
        alert(message);
    };

    return {
        populateWorksitesTable: populateWorksitesTable,
        reportError: reportError,
    };
}();