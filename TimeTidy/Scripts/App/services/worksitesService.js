"use strict";

var WorksitesService = function () {
    var getWorksites = function (done, fail) {
        return $.getJSON(
            "/api/worksites",
            { get_param: 'value' })
            .done(done)
            .fail(fail);
    };

    var deleteSite = function(siteId, success){
        $.ajax({
            url: "/api/worksites/" + siteId,
            method: "DELETE",
            success: success
        });
    };

    return {
        getWorksites: getWorksites,
        deleteSite: deleteSite,
    };
}();