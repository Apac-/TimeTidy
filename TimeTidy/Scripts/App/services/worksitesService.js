"use strict";

var WorksitesService = function () {
    var getWorksites = function (done, fail) {
        $.getJSON("/api/worksites", { get_param: 'value' })
            .done(done(data)).fail(fail);
    };

    return {
        getWorksites: getWorksites,
    };
}();