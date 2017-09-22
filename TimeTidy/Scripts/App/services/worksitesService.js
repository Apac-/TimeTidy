"use strict";

var WorksitesService = function () {
    var getWorksites = function (done, fail) {
        let deferredObj = $.getJSON("/api/worksites", { get_param: 'value' })
                                .done(done(data)).fail(fail);
        return deferredObj;
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