"use strict";

var WorksitesService = function () {
    var getWorksites = function (done, fail) {
        let deferredObj = $.getJSON("/api/worksites", { get_param: 'value' })
                                .done(done(data)).fail(fail);
        return deferredObj;
    };

    return {
        getWorksites: getWorksites,
    };
}();